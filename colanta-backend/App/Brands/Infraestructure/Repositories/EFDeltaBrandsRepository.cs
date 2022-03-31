namespace colanta_backend.App.Brands.Infraestructure
{
    using Brands.Domain;
    using Shared.Infraestructure;
    using Shared.Application;
    using System.Collections.Generic;
    using System;


    public class EFDeltaBrandsRepository : DeltaBrandsRepository
    {
        public colantaContext dbContext;
        public CustomConsole console;

        public EFDeltaBrandsRepository()
        {
            this.dbContext = new colantaContext();
            this.console = new CustomConsole();
        }

        public void deleteAllBrands()
        {
            this.dbContext.DeltaBrands.RemoveRange(this.dbContext.DeltaBrands);
            this.dbContext.SaveChanges();
            this.console.color(ConsoleColor.DarkGreen).write("Se limpió la tabla delta de marcas").reset();
        }

        public void fillBrands(Brand[] brands)
        {
            List<EFDeltaBrand> efDeltaBrands = new List<EFDeltaBrand>();
            foreach (Brand brand in brands) {
                EFDeltaBrand efDeltaBrand = new EFDeltaBrand();
                efDeltaBrand.setEFDeltaBrandFromBrand(brand);
                efDeltaBrands.Add(efDeltaBrand);
            }
            this.dbContext.DeltaBrands.AddRange(efDeltaBrands.ToArray());
            this.dbContext.SaveChanges();
            this.console.color(ConsoleColor.DarkGreen).write("Se llenó la tabla delta de marcas").reset();
        }

        public Brand[] getAllBrands()
        {
            var efDeltaBrands = this.dbContext.DeltaBrands;
            List<Brand> brands = new List<Brand>();
            foreach(EFDeltaBrand efDeltaBrand in efDeltaBrands)
            {
                brands.Add(efDeltaBrand.getBrandFromEFDeltaBrand());
            }
            return brands.ToArray();
        }
    }
}
