namespace colanta_backend.App.Brands.Application
{
    using App.Brands.Domain;
    public class GetBrandBySiesaId
    {
        private BrandsRepository locallyRepository;
        public GetBrandBySiesaId(BrandsRepository locallyRepository)
        {
            this.locallyRepository = locallyRepository;
        }
        public Brand? Invoke(string id)
        {
            return this.locallyRepository.getBrandBySiesaId(id);
        }
    }

    
}
