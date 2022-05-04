namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class SaveVtexSku
    {
        private SkusVtexRepository vtexRepository;

        public SaveVtexSku(SkusVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Sku> Invoke(Sku sku)
        {
            Sku vtexSku = await this.vtexRepository.getSkuBySiesaId(sku.ref_id);
            if (vtexSku != null)
            {
                return vtexSku;
            }
            return await this.vtexRepository.saveSku(sku);
        }
    }
}
