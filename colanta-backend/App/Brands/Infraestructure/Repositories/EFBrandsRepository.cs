

namespace colanta_backend.App.Brands.Infraestructure
{
    using App.Brands.Domain;
    using App.Shared.Infraestructure;
    using App.Brands.Infraestructure;
    using System.Linq;
    using System;

    public class EFBrandsRepository : BrandsRepository
    {
        private colantaContext dbContext;
        public EFBrandsRepository()
        {
            this.dbContext = new colantaContext();
        }
        public Brand[] getAllBrands()
        {
            Brand[] brands;
            EFBrand[] EFBrands = dbContext.Brands.ToArray();
            brands = EFBrands.Select(EFBrand => new Brand(
                    name: EFBrand.name, 
                    id_siesa: EFBrand.id_siesa, 
                    id: EFBrand.id
                )
            ).ToArray();
            return brands;
        }

        public Brand getBrandById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Brand? getBrandBySiesaId(string id)
        {
            var EFBrand = dbContext.Brands.Where(brand => brand.id_siesa == id);
            if (EFBrand.ToArray().Length == 0)
            {
                return null;
            }
            else
            {
                return EFBrand.First().getBrandFromEFBrand();
            }
        }

        public Brand getBrandByVtexId(int id)
        {
            throw new System.NotImplementedException();
        }

        public Brand saveBrand(Brand brand)
        {
            EFBrand EFBrand = new EFBrand();
            EFBrand.setEFBrandFromBrand(brand);
            dbContext.Brands.Add(EFBrand);
            dbContext.SaveChanges();
            return brand;
        }

        public Brand? updateBrand(Brand brand)
        {
            var efBrands = this.dbContext.Brands.Where(efBrand => efBrand.id == brand.id);
            if(efBrands.ToArray().Length != 0)
            {
                EFBrand efBrand = efBrands.First();
                efBrand.name = brand.name;
                efBrand.state = Convert.ToInt16(brand.state);
                efBrand.id_vtex = brand.id_vtex;
                dbContext.SaveChanges();
                return efBrand.getBrandFromEFBrand();
            }
            return null;
        }
    }
}
