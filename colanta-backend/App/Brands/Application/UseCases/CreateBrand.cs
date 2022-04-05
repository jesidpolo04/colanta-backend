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
            Brand localBrand =  this.locallyRepository.saveBrand(brand);
            this.console.color(ConsoleColor.Green).write("completado con éxito").reset();
            return localBrand;
        }

    }
}
