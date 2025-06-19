namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateSkus
    {
        private ISkusRepository localRepository;

        public UpdateSkus(ISkusRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Sku[]> Invoke(Sku[] skus)
        {
            return await this.localRepository.updateSkus(skus);
        }
    }
}
