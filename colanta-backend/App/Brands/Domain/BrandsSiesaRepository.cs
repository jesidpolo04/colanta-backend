using System.Threading.Tasks;

namespace colanta_backend.App.Brands.Domain
{
    public interface IBrandsSiesaRepository
    {
        public Task<Brand[]> getAllBrands();
    }
}
