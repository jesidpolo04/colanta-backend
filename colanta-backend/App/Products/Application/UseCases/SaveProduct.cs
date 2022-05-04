namespace colanta_backend.App.Products.Application
{
    using System.Threading.Tasks;
    using Products.Domain;
    public class SaveProduct
    {
        private ProductsRepository localRepository;

        public SaveProduct(ProductsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Product> Invoke(Product product)
        {
            return await this.localRepository.saveProduct(product);
        }
    }
}
