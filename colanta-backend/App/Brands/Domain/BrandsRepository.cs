namespace colanta_backend.App.Brands.Domain
{
    public interface BrandsRepository
    {
        Brand[] getAllBrands();
        Brand[] getDeltaBrands(Brand[] notInTheseBrands);
        Brand getBrandById(int id);
        Brand getBrandBySiesaId(string id);
        Brand getBrandByVtexId(int id);
        Brand saveBrand(Brand brand);
        Brand? updateBrand(Brand brand);
        Brand[] updateBrands(Brand[] brands);
    }
}
