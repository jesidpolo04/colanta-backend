namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateVtexProduct
    {
        private ProductsVtexRepository vtexRepository;

        public UpdateVtexProduct(ProductsVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Product> Invoke(Product product)
        {
            return await this.vtexRepository.updateProduct(product);
        }
    }
}
