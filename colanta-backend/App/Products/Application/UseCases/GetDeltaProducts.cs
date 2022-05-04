namespace colanta_backend.App.Products.Application
{
    using System.Threading.Tasks;
    using Products.Domain;
    public class GetDeltaProducts
    {
        private ProductsRepository localRepository;
        public GetDeltaProducts(ProductsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Product[]> Invoke(Product[] currentProducts)
        {
            return await this.localRepository.getDeltaProducts(currentProducts);
        }
    }
}
