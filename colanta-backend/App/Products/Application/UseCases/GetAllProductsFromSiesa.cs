namespace colanta_backend.App.Products.Application
{
    using System.Threading.Tasks;
    using Products.Domain;
    public class GetAllProductsFromSiesa
    {
        private ProductsSiesaRepository siesaRepository;
        public GetAllProductsFromSiesa(ProductsSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
        }

        public async Task<Product[]> Invoke()
        {
            return await this.siesaRepository.getAllProducts();
        }
    }
}
