namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateSkus
    {
        private SkusRepository localRepository;

        public UpdateSkus(SkusRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Sku[]> Invoke(Sku[] skus)
        {
            return await this.localRepository.updateSkus(skus);
        }
    }
}
