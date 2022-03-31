namespace colanta_backend.App.Brands.Application
{
    using App.Brands.Domain;
    using App.Shared.Application;
    public class UpdateBrands
    {
        private BrandsRepository brandsRepository;
        public UpdateBrands(BrandsRepository brandsRepository)
        {
            this.brandsRepository = brandsRepository;
        }

        public Brand[] Invoke(Brand[] brands)
        {
            return this.brandsRepository.updateBrands(brands);
        }
    }
}
