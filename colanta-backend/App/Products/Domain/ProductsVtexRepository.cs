namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public interface ProductsVtexRepository
    {
        void changeEnvironment(string environment);
        Task<Product> saveProduct(Product product);
        Task<Product?> getProductBySiesaId(string siesaId);
        Task<Product?> getProductByVtexId(string vtexId);
        Task<Product> updateProduct(Product product);
        Task associateProductToAStore(int vtexId, int storeId);
    }
}
