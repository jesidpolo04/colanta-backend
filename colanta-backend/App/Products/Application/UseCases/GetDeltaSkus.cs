namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class GetDeltaSkus
    {
        private SkusRepository localRepository;
        public GetDeltaSkus(SkusRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Sku[]> Invoke(Sku[] currentSkus)
        {
            return await this.localRepository.getDeltaSkus(currentSkus);
        }
    }
}
