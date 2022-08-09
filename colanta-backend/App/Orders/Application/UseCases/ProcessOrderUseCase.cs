namespace colanta_backend.App.Orders.Application
{
    using Orders.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class ProcessOrderUseCase
    {
        private OrdersRepository localRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;

        public ProcessOrderUseCase(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task Invoke(string vtexOrderId, string status, string lastStatus, string lastChange, string currentChange)
        {
            VtexOrder vtexOrder = await this.vtexRepository.getOrderByVtexId(vtexOrderId);
            Order localOrder = await this.localRepository.getOrderByVtexId(vtexOrderId);

            if (localOrder != null && localOrder.status == status) return; // si no cambió nada, finalizar

            if (localOrder != null && localOrder.status != status) //si cambió el estado ...
            {
                await this.localRepository.SaveOrderHistory(localOrder);
                await this.localRepository.deleteOrder(localOrder);

                localOrder.status = status;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.last_status = lastStatus;
                localOrder.last_change_date = lastChange;
                localOrder.current_change_date = currentChange;
            }
            
            if(localOrder == null) //si no existía
            {
                localOrder = new Order();
                localOrder.vtex_id = vtexOrderId;
                localOrder.order_json = JsonSerializer.Serialize(vtexOrder);
                localOrder.status = status;
                localOrder.last_status = lastStatus;
                localOrder.last_change_date = lastChange;
                localOrder.current_change_date = currentChange;
            }

            localOrder = await this.localRepository.SaveOrder(localOrder);

            PaymentMethod PAYMENT_METHOD = vtexOrder.getFirstPaymentMethod();
            string ORDER_STATUS = vtexOrder.status;

            if (PAYMENT_METHOD.isEqual(PaymentMethods.CONTRAENTREGA))
            {
                if (ORDER_STATUS.Equals(OrderVtexStates.PAYMENT_PENDING))
                {
                    SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
                    await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
                }
            }

            if (PAYMENT_METHOD.isEqual(PaymentMethods.EFECTIVO))
            {
                if (ORDER_STATUS.Equals(OrderVtexStates.PAYMENT_PENDING))
                {
                    SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
                    await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
                }
            }

            if (PAYMENT_METHOD.isEqual(PaymentMethods.CARD_PROMISSORY))
            {
                if (ORDER_STATUS.Equals(OrderVtexStates.PAYMENT_PENDING))
                {
                    SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
                    await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
                }
            }

            if (PAYMENT_METHOD.isEqual(PaymentMethods.CUSTOMER_CREDIT))
            {
                if (ORDER_STATUS.Equals(OrderVtexStates.READY_FOR_HANDLING))
                {
                    SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
                    await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
                }
            }

            if (PAYMENT_METHOD.isEqual(PaymentMethods.WOMPI))
            {
                if (ORDER_STATUS.Equals(OrderVtexStates.READY_FOR_HANDLING))
                {
                    SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
                    await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
                }
            }
        }
    }
}
