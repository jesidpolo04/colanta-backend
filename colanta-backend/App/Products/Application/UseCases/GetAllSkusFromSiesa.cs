namespace colanta_backend.App.Products.Application
{
    using System.Threading.Tasks;
    using Products.Domain;
    public class GetAllSkusFromSiesa
    {
        private ProductsSiesaRepository siesaRepository;
        public GetAllSkusFromSiesa(ProductsSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
        }

        public async Task<Sku[]> Invoke()
        {
            return await this.siesaRepository.getAllSkus();
        }
    }
}
