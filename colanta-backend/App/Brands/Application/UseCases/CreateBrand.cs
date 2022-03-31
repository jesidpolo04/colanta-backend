namespace colanta_backend.App.Brands.Application
{
    using Brands.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System.Threading.Tasks;
    using System;

    public class CreateBrand
    {
        private BrandsRepository locallyRepository;
        private ILogs logs;
        private CustomConsole console;

        public CreateBrand(BrandsRepository locallyRepository, ILogs logs )
        {
            this.locallyRepository = locallyRepository;
            this.logs = logs;
            this.console = new CustomConsole();
        }
        public Brand Invoke(Brand brand)
        {
            try
            {
                Brand localBrand =  this.locallyRepository.saveBrand(brand);
                this.console.color(ConsoleColor.DarkGreen).write("Se creó localmente la marca:")
                    .color(ConsoleColor.White).write("" + brand.name).skipLine();
                return localBrand;
            }
            catch (Exception exception)
            {
              return null;
            }
        }

    }
}
