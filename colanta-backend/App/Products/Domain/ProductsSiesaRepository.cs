namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public interface ProductsSiesaRepository
    {
        Task<Product[]> getAllProducts();
        Task<Sku[]> getAllSkus();
    }
}
