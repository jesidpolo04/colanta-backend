namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateProduct
    {
        private ProductsRepository localRepository;

        public UpdateProduct(ProductsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Product> Invoke(Product product)
        {
            return await this.localRepository.updateProduct(product);
        }
    }
}
