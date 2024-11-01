namespace colanta_backend.App.Orders.Application
{
    using System;
    using Products.Domain;
    using Users.Domain;
    using Shared.Domain;
    using Orders.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class SendOrderInAnyStatusUseCase
    {
        private OrdersRepository localRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private SkusRepository skusRepository;
        private MailService mailService;
        private RegisterUserService registerUserService;
        private EmailSender emailSender;

        public SendOrderInAnyStatusUseCase(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository,
            SkusRepository skusRepostory,
            MailService mailService,
            RegisterUserService registerUserService,
            EmailSender emailSender
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.skusRepository = skusRepostory;
            this.mailService = mailService;
            this.registerUserService = registerUserService;
            this.emailSender = emailSender;
        }

        public async Task Invoke(string vtexOrderId)
        {
            
            Order localOrder = await this.localRepository.getOrderByVtexId(vtexOrderId);
            VtexOrder vtexOrder = await this.vtexRepository.getOrderByVtexId(vtexOrderId);
            List<PaymentMethod> payments = vtexOrder.getPaymentMethods();

            if (localOrder != null && this.orderStatusHasBeenChanged(localOrder.status, vtexOrder.status))
            {
                await this.localRepository.SaveOrderHistory(localOrder);
                localOrder.last_status = localOrder.status;
                localOrder.status = vtexOrder.status;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.last_change_date = vtexOrder.lastChange.ToString(DateFormats.UTC);
                localOrder.current_change_date = DateTime.Now.ToString(DateFormats.UTC);
                localOrder = this.localRepository.updateOrder(localOrder).Result;
            }

            if (localOrder == null)
            {
                localOrder = new Order();
                localOrder.vtex_id = vtexOrderId;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.last_change_date = vtexOrder.lastChange.ToString(DateFormats.UTC);
                localOrder.last_status = localOrder.status;
                localOrder.status = vtexOrder.status;
                localOrder.current_change_date = DateTime.Now.ToString(DateFormats.UTC);
                localOrder = this.localRepository.SaveOrder(localOrder).Result;
            }

            if(!this.siesaOrderAlreadyExist(vtexOrderId))
            {
                this.emailSender.sendEmailWithoutTemplate($"Log orden {vtexOrder.orderId}", "Se debe enviar a SIESA", "jesing482@gmail.com");
                string userVtexId = vtexOrder.clientProfileData.userProfileId;
                string deliveryCountry = vtexOrder.shippingData.address.country;
                string deliveryDepartment = vtexOrder.shippingData.address.state;
                string deliveryCity = vtexOrder.shippingData.address.city;
                this.registerUser(userVtexId, deliveryCountry, deliveryDepartment, deliveryCity, vtexOrder.items[0].refId).Wait();
                SiesaOrder siesaOrder = await this.sendToSiesa(localOrder);
                this.notifyToStore(siesaOrder, vtexOrder.shippingData.logisticsInfo[0].polygonName);
                return;
            }
            this.emailSender.sendEmailWithoutTemplate($"Log orden {vtexOrder.orderId}", "No se debe enviar a SIESA", "jesing482@gmail.com");
        }

        private bool siesaOrderAlreadyExist(string vtexOrderId)
        {
            if (this.siesaOrdersLocalRepository.getSiesaOrderByVtexId(vtexOrderId).Result != null) return true;
            else return false;
        }

        private bool orderStatusHasBeenChanged(string oldStatus, string newStatus)
        {
            if (oldStatus != newStatus) return true;
            else return false;
        }
        

        private bool thereArePromissoryPayment(List<PaymentMethod> payments)
        {
            foreach(var payment in payments)
            {
                if (payment.isPromissory()) return true;
            }
            return false;
        }

        private async Task<SiesaOrder> sendToSiesa(Order order)
        {
            try
            {
                SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(order);
                await this.siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
                return siesaOrder;
            }
            catch(SiesaException exception)
            {
                if(!this.mailService.alreadyFailOrderMailSendedAtLast(2, "H", order.vtex_id))
                {
                    this.mailService.SendSiesaErrorMail(exception, order.vtex_id);
                    await this.mailService.createOrUpdateFailOrderMailLog(order.vtex_id);
                }
                throw new SiesaOrderRejectException(exception.httpResponse, order, $"Siesa rechazó el pedido #{order.vtex_id}");
            }
        }

        private void notifyToStore(SiesaOrder siesaOrder, string wharehouseId)
        {
            VtexOrder vtexOrder = this.vtexRepository.getOrderByVtexId(siesaOrder.referencia_vtex).Result;
            this.mailService.SendMailToWarehouse(wharehouseId, siesaOrder, vtexOrder);
           
        }

        private async Task registerUser(string userVtexId, string country, string department, string city, string someSkuRef)
        {

            string business = "";
            Sku sku = this.skusRepository.getSkuByConcatSiesaId(someSkuRef).Result;
            if (sku == null) business = "mercolanta";
            else business = sku.product.business;
            await this.registerUserService.registerUser(userVtexId, country, department, city, business);
        }
    }
}
