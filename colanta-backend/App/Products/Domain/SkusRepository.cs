namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public interface SkusRepository
    {
        Task<Sku> saveSku(Sku sku);
        Task<Sku?> getSkuBySiesaId(string siesaId);
        Task<Sku> getSkuByConcatSiesaId(string concatSiesaId);
        Task<Sku?> getSkuByVtexId(int vtexId);
        Task<Sku[]> getVtexNullSkus();
        Task<Sku[]> getVtexSkus();
        Task<Sku[]> getDeltaSkus(Sku[] currentSkus);
        Task<Sku> updateSku(Sku sku);
        Task<Sku[]> updateSkus(Sku[] skus);
    }
}
