namespace colanta_backend.App.Brands.Infraestructure
{
    using App.Brands.Domain;
    public class VtexBrandsRepository : BrandsVtexRepository
    {
        public Brand[] getAllBrands()
        {
            throw new System.NotImplementedException();
        }

        public Brand getBrandByVtexId(int id)
        {
            throw new System.NotImplementedException();
        }

        public Brand saveBrand(Brand brand)
        {
            return brand;
        }

        public Brand updateBrand(Brand brand)
        {
            throw new System.NotImplementedException();
        }
    }
}
