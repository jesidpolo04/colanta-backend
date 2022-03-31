using System.Threading.Tasks;

namespace colanta_backend.App.Brands.Domain
{
    public interface DeltaBrandsRepository
    {
        public Brand[] getAllBrands();
        public void deleteAllBrands();
        public void fillBrands(Brand[] brands);
    }
}
