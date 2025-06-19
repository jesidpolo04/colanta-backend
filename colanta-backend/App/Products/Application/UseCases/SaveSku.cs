namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class SaveSku
    {
        private ISkusRepository localRepository;

        public SaveSku(ISkusRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Sku> Invoke(Sku sku)
        {
            return await this.localRepository.saveSku(sku);
        }
    }
}
