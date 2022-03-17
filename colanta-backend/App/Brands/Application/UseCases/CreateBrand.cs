namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    public class CreateBrand
    {
        private BrandsRepository locallyRepository;
        private BrandsVtexRepository vtexRepository;


        public CreateBrand(BrandsRepository locallyRepository, BrandsVtexRepository vtexRepository)
        {
            this.locallyRepository = locallyRepository;
            this.vtexRepository = vtexRepository;
        }
        public Brand Invoke(Brand brand)
        {
            this.locallyRepository.saveBrand(brand);
            this.vtexRepository.saveBrand(brand);
            return brand;
        }

    }
}
