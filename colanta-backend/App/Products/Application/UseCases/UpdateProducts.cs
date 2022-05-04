namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateProducts
    {
        private ProductsRepository localRepository;

        public UpdateProducts(ProductsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Product[]> Invoke(Product[] products)
        {
            return await this.localRepository.updateProducts(products);
        }
    }
}
