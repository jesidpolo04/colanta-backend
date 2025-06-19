namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class GetSkuBySiesaId
    {
        private ISkusRepository localRepository;

        public GetSkuBySiesaId(ISkusRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Sku> Invoke(string siesaId)
        {
            return await this.localRepository.getSkuBySiesaId(siesaId);
        }
    }
}
