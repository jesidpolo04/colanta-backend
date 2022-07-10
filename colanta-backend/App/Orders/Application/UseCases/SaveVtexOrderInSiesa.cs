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
            Order localOrder = await this.localRepository.SaveOrder(order);
            SiesaOrder siesaOrder = await this.siesaRepository.saveOrder(localOrder);
            return await siesaOrdersLocalRepository.saveSiesaOrder(siesaOrder);
        }
    }
}
