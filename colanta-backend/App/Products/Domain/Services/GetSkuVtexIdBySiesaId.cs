namespace colanta_backend.App.Products.Domain
{
    using System.Threading.Tasks;
    public class GetSkuVtexIdBySiesaId
    {
        private ISkusRepository skusLocalRepository;
        public GetSkuVtexIdBySiesaId(ISkusRepository skusLocalRepository)
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
