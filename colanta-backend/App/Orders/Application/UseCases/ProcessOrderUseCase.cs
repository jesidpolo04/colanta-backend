namespace colanta_backend.App.Orders.Application
{
    using System;
    using Users.Domain;
    using Shared.Domain;
    using Orders.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class ProcessOrderUseCase
    {
        private OrdersRepository localRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private MailService mailService;
        private RegisterUserService registerUserService;

        public ProcessOrderUseCase(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository,
            MailService mailService,
            RegisterUserService registerUserService
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.mailService = mailService;
            this.registerUserService = registerUserService;
        }

        public async Task Invoke(string vtexOrderId, string status, string lastStatus, string lastChange, string currentChange)
        {
            
            Order localOrder = await this.localRepository.getOrderByVtexId(vtexOrderId);
            VtexOrder vtexOrder = await this.vtexRepository.getOrderByVtexId(vtexOrderId);
            List<PaymentMethod> payments = vtexOrder.getPaymentMethods();

            if (localOrder != null && this.orderStatusHasBeenChanged(localOrder.status, status))
            {
                await this.localRepository.SaveOrderHistory(localOrder);
                localOrder.status = status;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.last_status = lastStatus;
                localOrder.last_change_date = lastChange;
                localOrder.current_change_date = currentChange;
                localOrder = this.localRepository.updateOrder(localOrder).Result;
            }

            if (localOrder == null)
            {
                localOrder = new Order();
                localOrder.vtex_id = vtexOrderId;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.status = status;
                localOrder.last_status = lastStatus;
                localOrder.last_change_date = lastChange;
                localOrder.current_change_date = currentChange;
                localOrder = this.localRepository.SaveOrder(localOrder).Result;
            }

            if(this.mustToSendToSiesa(payments, vtexOrder.status) && !this.siesaOrderAlreadyExist(vtexOrderId))
            {
                string userVtexId = vtexOrder.clientProfileData.userProfileId;
                string deliveryCountry = vtexOrder.shippingData.address.country;
                string deliveryDepartment = vtexOrder.shippingData.address.state;
                string deliveryCity = vtexOrder.shippingData.address.city;

                this.registerUser(userVtexId, deliveryCountry, deliveryDepartment, deliveryCity).Wait();
                SiesaOrder siesaOrder = await this.sendToSiesa(localOrder);
                this.notifyToStore(siesaOrder, vtexOrder.shippingData.logisticsInfo[0].polygonName);
            }
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

        private bool mustToSendToSiesa(List<PaymentMethod> payments, string status)
        {
            if (status == OrderVtexStates.READY_FOR_HANDLING)
            {
                if (!thereArePromissoryPayment(payments))
                {
                    return true;
                }
            }
            if (status == OrderVtexStates.PAYMENT_PENDING)
            {
                if (thereArePromissoryPayment(payments))
                {
                    return true;
                }
            }
            return false;
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
                this.mailService.SendSiesaErrorMail(exception, order.vtex_id);
                throw new SiesaOrderRejectException(exception.httpResponse, order, $"Siesa rechazó el pedido #{order.vtex_id}");
            }
        }

        private void notifyToStore(SiesaOrder order, string wharehouseId)
        {
            try
            {
                this.mailService.SendMailToWarehouse(wharehouseId, order);
            }
            catch(Exception exception)
            {
                Console.WriteLine($"Error al enviar el mail a la tienda: {exception.Message}");
            } 
        }

        private async Task registerUser(string userVtexId, string country, string department, string city)
        {
            try
            {
                await this.registerUserService.registerUser(userVtexId, country, department, city);
            }
            catch (SiesaException exception)
            {
                Console.WriteLine("Error en al intentar registrar al cliente en siesa");
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.requestBody);
                Console.WriteLine(exception.responseBody);
            }
            catch(VtexException exception)
            {
                Console.WriteLine("Error en al consultar el cliente en vtex");
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.requestBody);
                Console.WriteLine(exception.responseBody);
            }
        }
    }
}
