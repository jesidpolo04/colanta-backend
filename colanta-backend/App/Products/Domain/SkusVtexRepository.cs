namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public interface SkusVtexRepository
    {
        void changeEnvironment(string environment);
        Task<Sku> saveSku(Sku sku);
        Task<Sku?> getSkuByInVtexRef(string siesaId);
        Task<Sku?> getSkuByVtexId(string vtexId);
        Task<Sku> updateSku(Sku sku);
    }
}
