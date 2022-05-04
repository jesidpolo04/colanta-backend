namespace colanta_backend.App.Products.Application
{
    using System.Threading.Tasks;
    using Products.Domain;
    public class GetVtexSkuBySiesaId
    {
        private SkusVtexRepository vtexRepository;

        public GetVtexSkuBySiesaId(SkusVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Sku?> Invoke(string siesaId)
        {
            return await this.vtexRepository.getSkuBySiesaId(siesaId);
        }
    }
}
