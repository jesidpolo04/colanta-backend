namespace colanta_backend.App.Brands.Domain
{
    public interface BrandsVtexRepository
    {
        Brand saveBrand(Brand brand);
        Brand getBrandByVtexId(int id);
        Brand[] getAllBrands();
        Brand updateBrand(Brand brand);
    }
}
