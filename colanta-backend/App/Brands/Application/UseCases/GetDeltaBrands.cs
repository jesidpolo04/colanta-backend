namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    public class GetDeltaBrands
    {
        private BrandsRepository brandsRepository;

        public GetDeltaBrands(BrandsRepository brandsRepository)
        {
            this.brandsRepository = brandsRepository;
        }

        public Brand[] Invoke(Brand[] currentBrands)
        {
            return this.brandsRepository.getDeltaBrands(currentBrands);
        }

    }
}
