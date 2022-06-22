namespace colanta_backend.App.Orders.Application
{
    using Orders.Domain;
    using System.Threading.Tasks;
    public class SaveVtexOrderInSiesa
    {
        private OrdersRepository localRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;

        public SaveVtexOrderInSiesa(
            OrdersRepository localRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task<Order> Invoke(Order order)
        {
            string orderJson = await this.vtexRepository.getOrderByVtexId(order.vtex_id);
            
            order.order_json = orderJson;
            Order localOrder = await this.localRepository.SaveOrder(order);
            return await this.siesaRepository.saveOrder(localOrder);
        }
    }
}
