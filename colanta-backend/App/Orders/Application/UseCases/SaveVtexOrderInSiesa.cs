namespace colanta_backend.App.Orders.Application
{
    using Orders.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Threading.Tasks;
    public class SaveVtexOrderInSiesa
    {
        private OrdersRepository localRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;

        public SaveVtexOrderInSiesa(
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

        public async Task<SiesaOrder> Invoke(Order order)
        {
            string orderJson = await this.vtexRepository.getOrderByVtexId(order.vtex_id);
            order.order_json = orderJson;

            Order localOrder = await this.localRepository.getOrderByVtexId(order.vtex_id);
            if(localOrder != null)
            {
                await this.localRepository.SaveOrderHistory(localOrder);
                await this.localRepository.deleteOrder(localOrder);
            }

            localOrder = await this.localRepository.SaveOrder(order);
            SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
            PaymentMethod paymentMethod = await this.vtexRepository.getOrderPaymentMethod(siesaOrder.referencia_vtex);
            OrderStatus orderStatus = await this.vtexRepository.getOrderStatus(siesaOrder.referencia_vtex);
            siesaOrder.estado_vtex = orderStatus.status;
            siesaOrder.metodo_pago_vtex = paymentMethod.name;
            siesaOrder.id_metodo_pago_vtex = paymentMethod.vtex_id;
            return await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
        }
    }
}
