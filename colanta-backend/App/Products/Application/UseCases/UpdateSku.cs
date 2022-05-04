namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateSku
    {
        private SkusRepository localRepository;

        public UpdateSku(SkusRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Sku> Invoke(Sku sku)
        {
            return await this.localRepository.updateSku(sku);
        }
    }
}
