namespace colanta_backend.App.Products.Application
{
    using Products.Domain;
    using System.Threading.Tasks;
    public class UpdateVtexSku
    {
        private SkusVtexRepository vtexRepository;

        public UpdateVtexSku(SkusVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Sku> Invoke(Sku sku)
        {
            return await this.vtexRepository.updateSku(sku);
        }
    }
}
