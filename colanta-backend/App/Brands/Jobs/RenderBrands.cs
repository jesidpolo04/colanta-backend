namespace colanta_backend.App.Brands.Jobs
{
    using App.Brands.Application;
    using App.Brands.Domain;
    using App.Shared.Domain;
    using App.Brands.Infraestructure;
    using App.Shared.Application;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class RenderBrands
    {
        public string processName = "Renderizado de marcas";
        private BrandsRepository brandsLocalRepository;
        private BrandsVtexRepository brandsVtexRepository;
        private ILogs logs;
        private EmailSender emailSender;

        private CustomConsole console;
        
        private List<Detail> details;
        private List<Brand> loadBrands;
        private List<Brand> inactiveBrands;
        private List<Brand> failedLoadBrands;
        private List<Brand> notProccecedBrands;
        private List<Brand> inactivatedBrands;
        

        private int total_loads = 0;
        private int total_errors = 0;
        private int total_not_procecced = 0;

        public RenderBrands(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository, ILogs logs, EmailSender emailSender)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
            this.logs = logs;
            this.emailSender = emailSender;

            this.console = new CustomConsole();
            this.details = new List<Detail>();

            this.inactiveBrands = new List<Brand>();
            this.loadBrands = new List<Brand>();
            this.failedLoadBrands = new List<Brand>();
            this.notProccecedBrands = new List<Brand>();
            this.inactivatedBrands = new List<Brand>();

            this.console.warningColor().write("Iniciando proceso:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();
        }

        public async Task<bool> Invoke()
        {
            BrandsSiesaRepository brandsSiesaRepository = new BrandsSiesaRepository();
            GetDeltaBrands getDeltaBrands = new GetDeltaBrands(this.brandsLocalRepository);
            GetBrandBySiesaId getBrandBySiesaId = new GetBrandBySiesaId(this.brandsLocalRepository);
            CreateBrand createBrand = new CreateBrand(this.brandsLocalRepository);
            CreateVtexBrand createVtexBrand = new CreateVtexBrand(this.brandsVtexRepository);
            UpdateVtexBrand updateVtexBrand = new UpdateVtexBrand(this.brandsVtexRepository);
            UpdateBrands updateBrands = new UpdateBrands(this.brandsLocalRepository);
            UpdateBrand updateBrand = new UpdateBrand(this.brandsLocalRepository);

            Brand[] siesaBrands = await brandsSiesaRepository.getAllBrands();
            this.details.Add(new Detail(
                    origin: "siesa",
                    success: true,
                    description: "Petición para obtener todas las marcas de siesa, completada con éxito"
                ));

            Brand[] deltaBrands = getDeltaBrands.Invoke(siesaBrands);
            if (deltaBrands.Length > 0)
            {
                foreach (Brand deltaBrand in deltaBrands)
                {
                    deltaBrand.state = false;
                    updateVtexBrand.Invoke(deltaBrand);
                    this.inactivatedBrands.Add(deltaBrand);
                }
                updateBrands.Invoke(deltaBrands);
            }

            foreach (Brand siesaBrand in siesaBrands)
            {
                try
                {
                    Brand? localBrand = getBrandBySiesaId.Invoke(siesaBrand.id_siesa);

                    if (localBrand != null && localBrand.state == true)
                    {
                        //all right
                        this.notProccecedBrands.Add(siesaBrand);
                    }

                    if (localBrand != null && localBrand.state == false && localBrand.id_vtex != null)
                    {
                        //not yet active brand
                        this.inactiveBrands.Add(localBrand);
                    }

                    if (localBrand == null)
                    {
                        localBrand = createBrand.Invoke(siesaBrand);
                        Brand? vtexBrand = await createVtexBrand.Invoke(localBrand);

                        this.details.Add(
                            new Detail(
                                origin: "vtex",
                                success: true,
                                description: "request for create a brand in VTEX, completed successfully"
                        ));

                        localBrand.id_vtex = vtexBrand.id_vtex;
                        updateBrand.Invoke(localBrand);
                        this.loadBrands.Add(vtexBrand);
                    }

                }
                catch (VtexException vtexException)
                {
                    this.details.Add(new Detail(
                                            origin: "vtex",
                                            success: false,
                                            description: vtexException.Message
                                        ));
                    this.failedLoadBrands.Add(siesaBrand);
                    this.total_errors++;
                }
                catch (Exception exception)
                {
                    //normalmente problemas de conexión del middleware
                }
            }

            this.logs.Log(
                    name: "Renderizado de Marcas",
                    total_loads: this.total_loads,
                    total_errors: this.total_errors,
                    total_not_procecced: this.total_not_procecced,
                    json_details: JsonSerializer.Serialize(this.details.ToArray())
                );




            //refactor the below code ↓ ↓ ↓

            string emailMessage = "";
            emailMessage += "<h3>Marcas pendientes de activación</h3>";
            emailMessage += "<ul>";
            foreach (Brand inactiveBrand in this.inactiveBrands)
            {
                emailMessage += "<li>" + inactiveBrand.name + "</li>";
            }
            emailMessage += "</ul>";

            emailMessage += "<h3>Nuevas Marcas</h3>";
            emailMessage += "<ul>";
            foreach (Brand loadBrand in this.loadBrands)
            {
                emailMessage += "<li>" + loadBrand.name + "</li>";
            }
            emailMessage += "</ul>";

            emailMessage += "<h3>Excepciones</h3>";
            emailMessage += "<ul>";
            foreach (Brand failedloadBrand in this.failedLoadBrands)
            {
                emailMessage += "<li>" + failedloadBrand.name + "</li>";
            }
            emailMessage += "</ul>";

            this.writeConsoleLogs();
            this.emailSender.SendEmail(emailMessage);

            this.console.warningColor().write("Proceso Finalizado:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

            return true;
        }

        private void writeConsoleLogs()
        {
            if (this.inactivatedBrands.Count > 0)
            {
                this.console.errorColor().writeLine("Marcas desactivadas");
                foreach(Brand inactivatedBrand in this.inactivatedBrands)
                {
                    this.console.whiteColor().write(inactivatedBrand.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(inactivatedBrand.id_siesa)
                        .grayColor().write("vtex id:")
                        .infoColor().write(inactivatedBrand.id_vtex.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.loadBrands.Count > 0)
            {
                this.console.successColor().writeLine("Marcas Creadas");
                foreach (Brand loadBrand in this.loadBrands)
                {
                    this.console.whiteColor().write(loadBrand.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(loadBrand.id_siesa)
                        .grayColor().write("vtex id:")
                        .infoColor().write(loadBrand.id_vtex.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.failedLoadBrands.Count > 0)
            {
                this.console.errorColor().writeLine("Marcas no subidas a VTEX debido a error");
                foreach (Brand failedBrand in this.failedLoadBrands)
                {
                    this.console.whiteColor().write(failedBrand.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(failedBrand.id_siesa)
                        .grayColor().write("vtex id:")
                        .infoColor().write(failedBrand.id_vtex.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.inactiveBrands.Count > 0)
            {
                this.console.warningColor().writeLine("Marcas por activar en VTEX");
                foreach (Brand inactiveBrand in this.inactiveBrands)
                {
                    this.console.whiteColor().write(inactiveBrand.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(inactiveBrand.id_siesa)
                        .grayColor().write("vtex id:")
                        .infoColor().write(inactiveBrand.id_vtex.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.notProccecedBrands.Count > 0)
            {
                this.console.successColor().writeLine("Marcas no procesadas");
                foreach (Brand notProccecedBrand in this.notProccecedBrands)
                {
                    this.console.whiteColor().write(notProccecedBrand.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(notProccecedBrand.id_siesa)
                        .grayColor().write("vtex id:")
                        .infoColor().write(notProccecedBrand.id_vtex.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }
        }
    }
}
