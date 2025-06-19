namespace colanta_backend.App.Orders.Domain
{
    using Products.Domain;
    using SiesaOrders.Domain;
    using System;
    using System.Threading.Tasks;
    public class GetOrderDetailsVtexId
    {
        private ISkusRepository skusLocalRepository;
        public GetOrderDetailsVtexId(ISkusRepository skusLocalRepository)
        {
            this.skusLocalRepository = skusLocalRepository;
        }

        public async Task Invoke(SiesaOrder siesaOrder)
        {
            foreach(SiesaOrderDetail siesaOrderDetail in siesaOrder.detalles)
            {
                Sku product = await this.skusLocalRepository.getSkuBySiesaId(siesaOrderDetail.referencia_item);
                siesaOrderDetail.referencia_vtex = product.vtex_id.ToString();
            }
        }
    }
}
