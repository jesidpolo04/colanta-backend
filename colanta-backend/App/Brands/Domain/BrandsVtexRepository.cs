using System.Threading.Tasks;

namespace colanta_backend.App.Brands.Domain
{
    public interface BrandsVtexRepository
    {
        Task<Brand> saveBrand(Brand brand);
        Task<Brand> getBrandByVtexId(int? id);
        Task<Brand[]> getAllBrands();
        Task<Brand> updateBrand(Brand brand);
    }
}
