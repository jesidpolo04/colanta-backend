namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public interface ProductsRepository
    {
        Task<Product> saveProduct(Product product);
        Task<Product?> getProductBySiesaId(string siesaId);
        Task<Product?> getProductByVtexId(int vtexId);
        Task<Product[]> getVtexNullProducts();
        Task<Product[]> getVtexProducts();
        Task<Product[]> getDeltaProducts(Product[] currentProducts);
        Task<Product> updateProduct(Product product);
        Task<Product[]> updateProducts(Product[] products);
    }
}
