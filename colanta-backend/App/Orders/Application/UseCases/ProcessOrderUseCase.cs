namespace colanta_backend.App.Orders.Application
{
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

        public ProcessOrderUseCase(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository,
            MailService mailService
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.mailService = mailService;
        }

        public async Task Invoke(string vtexOrderId, string status, string lastStatus, string lastChange, string currentChange)
        {
            VtexOrder vtexOrder = await this.vtexRepository.getOrderByVtexId(vtexOrderId);
            Order localOrder = await this.localRepository.getOrderByVtexId(vtexOrderId);

            if (localOrder != null && localOrder.status == status) return; // si no cambió nada, finalizar

            if (localOrder != null && localOrder.status != status) //si cambió el estado ...
            {
                await this.localRepository.SaveOrderHistory(localOrder);
                localOrder.status = status;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.last_status = lastStatus;
                localOrder.last_change_date = lastChange;
                localOrder.current_change_date = currentChange;
                localOrder = this.localRepository.updateOrder(localOrder).Result;
            }

            if (localOrder == null) //si no existía
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

            if (status == OrderVtexStates.READY_FOR_HANDLING)
            {
                if (!thereArePromissoryPayment(vtexOrder.getPaymentMethods()))
                {
                    this.notifyToStore(vtexOrderId, vtexOrder.shippingData.logisticsInfo[0].addressId);
                    await this.sendToSiesa(vtexOrder.getPaymentMethods(), localOrder);
                 }
            }
            if (status == OrderVtexStates.PAYMENT_PENDING)
            {
                if (thereArePromissoryPayment(vtexOrder.getPaymentMethods()))
                {
                    this.notifyToStore(vtexOrderId, vtexOrder.shippingData.logisticsInfo[0].addressId);
                    await this.sendToSiesa(vtexOrder.getPaymentMethods(), localOrder);
                }
            }
        }

        

        private bool thereArePromissoryPayment(List<PaymentMethod> payments)
        {
            foreach(var payment in payments)
            {
                if (payment.isPromissory()) return true;
            }
            return false;
        }

        private async Task sendToSiesa(List<PaymentMethod> payments, Order order)
        {
            try
            {
                await this.siesaRepository.saveOrder(order);
            }
            catch
            {
            }
        }

        private void notifyToStore(string orderId, string wharehouseId)
        {
            try
            {
                this.mailService.SendMailToWarehouse(orderId, wharehouseId);
            }
            catch
            {
            } 
        }
    }
}
