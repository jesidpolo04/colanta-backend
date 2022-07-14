namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public class GetSkuVtexIdBySiesaId
    {
        private SkusRepository skusLocalRepository;
        public GetSkuVtexIdBySiesaId(SkusRepository skusLocalRepository)
        {
            this.skusLocalRepository = skusLocalRepository;
        }

        public async Task<string> Invoke(string siesaId)
        {
            Sku sku = await this.skusLocalRepository.getSkuBySiesaId(siesaId);
            return sku.vtex_id.ToString();
        }
    }
}
