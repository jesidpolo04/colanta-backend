namespace colanta_backend.App.Brands.Application
{
    using System.Threading.Tasks;
    using System;
    using Brands.Domain;
    using Shared.Domain;
    using Shared.Application;

    public class CreateVtexBrand
    {
        private BrandsVtexRepository _brandsVtexRepository;
        private int _numberOfTry;
        private CustomConsole console;
        public CreateVtexBrand(BrandsVtexRepository brandsVtexRepository)
        {
            this._numberOfTry = 5;
            this._brandsVtexRepository = brandsVtexRepository;
            this.console = new CustomConsole();
        }

        public async Task<Brand?> Invoke(Brand brand)
        {
            try
            {
                Brand vtexBrand = await this._brandsVtexRepository.saveBrand(brand);
                this.console
                    .color(ConsoleColor.DarkGreen).write("Se creó en VTEX la marca:")
                    .color(ConsoleColor.White).write(""+brand.name).reset();
                return vtexBrand;
            }
            catch(Exception vtexExcepcion)
            {
                this.console.color(ConsoleColor.Red).write("Excepción:")
                    .color(ConsoleColor.White).write(vtexExcepcion.Message).skipLine();
                Brand? vtexBrand = null;
                for(int i = 1; i <= this._numberOfTry; i++)
                {
                    this.console.color(ConsoleColor.Yellow).write("Reintentando inserción, intento:")
                        .color(ConsoleColor.White).write("" + (i)).skipLine();
                    try
                    {
                        vtexBrand = await this._brandsVtexRepository.saveBrand(brand);
                        this.console.color(ConsoleColor.DarkGreen).write("Inserción a VTEX exitosa:")
                            .color(ConsoleColor.White).write(""+(i)).dot(2);
                        break;
                    }catch(VtexException tryException)
                    {
                        if(i == (this._numberOfTry))
                        {
                            throw tryException;
                        }
                    }
                }
                return vtexBrand;
            }
        }
    }
}
