

namespace colanta_backend.App.Brands.Infraestructure
{
    using App.Brands.Domain;
    using App.Shared.Infraestructure;
    using App.Shared.Application;
    using App.Brands.Infraestructure;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    public class EFBrandsRepository : BrandsRepository
    {
        private colantaContext dbContext;
        private CustomConsole console;
        public EFBrandsRepository(IConfiguration configuration)
        {
            this.dbContext = new colantaContext(configuration);
            this.console = new CustomConsole();
        }
        public Brand[] getAllBrands()
        {
            Brand[] brands;
            EFBrand[] EFBrands = dbContext.Brands.ToArray();
            brands = EFBrands.Select(EFBrand => new Brand(
                    name: EFBrand.name, 
                    id_siesa: EFBrand.id_siesa,
                    id_vtex: EFBrand.id_vtex,
                    id: EFBrand.id,
                    state: Convert.ToBoolean( EFBrand.state )
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

        public Brand[] getDeltaBrands(Brand[] notInTheseBrands)
        {
            string[] notInTheseIds = notInTheseBrands.Select(notInThisBrand => notInThisBrand.id_siesa).ToArray();
            EFBrand[] efBrands = this.dbContext.Brands.Where(brand => !notInTheseIds.Contains(brand.id_siesa) && brand.state == 1).ToArray(); //falta un to array
            List<Brand> brands = new List<Brand>();
            if(efBrands.Length == 0)
            {
                return brands.ToArray();
            }
            foreach (EFBrand efBrand in efBrands)
            {
                brands.Add(efBrand.getBrandFromEFBrand());
            }
            return brands.ToArray();
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
                this.console.color(ConsoleColor.DarkGreen).write("Se actualizó correctamente la marca:")
                    .color(ConsoleColor.White).write("" + brand.name).reset(); 
                return efBrand.getBrandFromEFBrand();
            }
            
            return null;
        }

        public Brand[] updateBrands(Brand[] brands)
        {
            foreach(Brand brand in brands)
            {
                EFBrand efBrand = this.dbContext.Brands.Find(brand.id);
                efBrand.setEFBrandFromBrand(brand);
            }
            dbContext.SaveChanges();
            return brands;
        }
    }
}
