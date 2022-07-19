namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    using Shared.Domain;
    using System.Threading.Tasks;

    public class GetVtexBrandByVtexId
    {
        BrandsVtexRepository brandsVtexRepository;
        public GetVtexBrandByVtexId(BrandsVtexRepository brandsVtexRepository)
        {
            this.brandsVtexRepository = brandsVtexRepository;
        }

        public async Task<Brand> Invoke(Brand brand)
        {
            if(brand.id_vtex == null)
            {
                return null;
            }
            return await this.brandsVtexRepository.getBrandByVtexId(brand.id_vtex);
        }
    }
}
