namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    using System.Threading.Tasks;
    public class UpdateVtexBrand
    {
        private BrandsVtexRepository brandsVtexRepository;
        public UpdateVtexBrand(BrandsVtexRepository brandsVtexRepository)
        {
            this.brandsVtexRepository = brandsVtexRepository;
        }

        public Task<Brand> Invoke(Brand brand)
        {
            return this.brandsVtexRepository.updateBrand(brand);
        }
    }
}
