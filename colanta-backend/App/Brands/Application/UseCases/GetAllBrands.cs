namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    public class GetAllBrands
    {
        private BrandsRepository locallyRepository;

        public GetAllBrands(BrandsRepository locallyRepository)
        {
            this.locallyRepository = locallyRepository;
        }
        public Brand[] Invoke()
        {
            return this.locallyRepository.getAllBrands();
        }

    }
}
