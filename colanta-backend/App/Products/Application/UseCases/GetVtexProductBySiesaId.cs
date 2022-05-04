namespace colanta_backend.App.Products.Application
{
    using System.Threading.Tasks;
    using Products.Domain;
    public class GetVtexProductBySiesaId
    {
        private ProductsVtexRepository vtexRepository;

        public GetVtexProductBySiesaId(ProductsVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Product?> Invoke(string siesaId)
        {
            return await this.vtexRepository.getProductBySiesaId(siesaId);
        }
    }
}
