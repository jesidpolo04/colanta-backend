namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    public class UpdateBrand
    {
        private BrandsRepository locallyRepository;
        public UpdateBrand(BrandsRepository locallyRepository)
        {
            this.locallyRepository = locallyRepository;
        }

        public Brand? Invoke(Brand brand)
        {
            return this.locallyRepository.updateBrand(brand);
        }

    }
}
