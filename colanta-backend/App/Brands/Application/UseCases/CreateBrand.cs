namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;

    public class CreateBrand
    {
        private BrandsRepository locallyRepository;

        public CreateBrand(BrandsRepository locallyRepository)
        {
            this.locallyRepository = locallyRepository;
        }
        public Brand Invoke(Brand brand)
        {
            Brand localBrand =  this.locallyRepository.saveBrand(brand);
            return localBrand;
        }

    }
}
