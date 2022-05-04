namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class SaveVtexProduct
    {
        private ProductsVtexRepository vtexRepository;

        public SaveVtexProduct(ProductsVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Product> Invoke(Product product)
        {
            Product vtexProduct = await this.vtexRepository.getProductBySiesaId(product.ref_id);
            if(vtexProduct != null)
            {
                return vtexProduct;
            }
            return await this.vtexRepository.saveProduct(product);
        }
    }
}
