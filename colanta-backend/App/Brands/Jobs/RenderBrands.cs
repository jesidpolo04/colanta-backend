namespace colanta_backend.App.Brands.Jobs
{
    using App.Brands.Application;
    using App.Brands.Domain;
    using App.Shared.Domain;
    using App.Brands.Infraestructure;
    using App.Shared.Application;
    using App.Shared.Infraestructure;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    public class RenderBrands
    {
        private GetBrandBySiesaId getBrandBySiesaId;
        private CreateBrand createBrand;
        private CreateVtexBrand createVtexBrand;
        private IBrandsSiesaRepository brandsSiesaRepository;
        private GetDeltaBrands getDeltaBrands;
        private UpdateBrands updateBrands;
        private UpdateBrand updateBrand;
        private CustomConsole console;
        private ILogs logs;
        private List<Detail> details;
        private EmailSender emailSender;

        private int total_loads = 0;
        private int total_errors = 0;
        private int total_not_procecced = 0;

        public RenderBrands()
        {
            this.getBrandBySiesaId = new GetBrandBySiesaId(new EFBrandsRepository());
            this.createBrand = new CreateBrand(new EFBrandsRepository(), new ProcessLogs());
            this.createVtexBrand = new CreateVtexBrand(new VtexBrandsRepository());
            this.brandsSiesaRepository = new BrandsSiesaRepository();
            this.getDeltaBrands = new GetDeltaBrands(new EFBrandsRepository());
            this.updateBrands = new UpdateBrands(new EFBrandsRepository());
            this.updateBrand = new UpdateBrand(new EFBrandsRepository());
            this.emailSender = new GmailSender();
            this.console = new CustomConsole();
            this.logs = new ProcessLogs();
            this.details = new List<Detail>();

            this.console.color(ConsoleColor.Yellow).write("Inicio de ejecución del cron").dot(2);
        }

        public async void Invoke()
        {
            Brand[] siesaBrands = await this.brandsSiesaRepository.getAllBrands();
            this.details.Add(new Detail(
                origin: "siesa",
                success: true,
                description: "request for get all brands in siesa, completed successfully"
                )); ;

            Brand[] deltaBrands = this.getDeltaBrands.Invoke(siesaBrands);
            if(deltaBrands.Length > 0)
            {
                this.console.color(ConsoleColor.Yellow).write("Esta vez no vinieron")
                    .color(ConsoleColor.White).write("" + deltaBrands.Length)
                    .color(ConsoleColor.Yellow).write("de las marcas que ya tenemos activas en local").reset();

                foreach(Brand deltaBrand in deltaBrands)
                {
                    deltaBrand.state = false;
                }
                this.updateBrands.Invoke(deltaBrands);
            }

            foreach (Brand siesaBrand in siesaBrands)
            {
                try
                {
                    Brand? localBrand = this.getBrandBySiesaId.Invoke(siesaBrand.id_siesa);

                    if(localBrand != null && localBrand.state == true)
                    {
                        this.total_not_procecced++;
                    }

                    if(localBrand != null && localBrand.state == false)
                    {
                        localBrand.state = true;
                        this.updateBrand.Invoke(localBrand);
                        this.total_not_procecced++;
                    }

                    if (localBrand == null)
                    {
                        //Create brand if no exists
                        localBrand = this.createBrand.Invoke(siesaBrand);
                        Brand? vtexBrand = await this.createVtexBrand.Invoke(localBrand);
                        this.details.Add(new Detail(
                                                origin: "vtex",
                                                success: true,
                                                description: "request for create a brand in VTEX, completed successfully"
                                            )); 
                        localBrand.id_vtex = vtexBrand.id_vtex;
                        UpdateBrand updateBrand = new UpdateBrand(new EFBrandsRepository());
                        updateBrand.Invoke(localBrand);
                        this.total_loads++;
                    }

                }
                catch (VtexException vtexException) 
                {
                    this.details.Add(new Detail(
                                            origin: "vtex",
                                            success: false,
                                            description: vtexException.Message
                                        ));
                    this.total_errors++;
                    this.emailSender.SendEmail("¡Alerta!, no fue posible crear en vtex la marca: " + siesaBrand.name);
                }      
                catch(Exception exception)
                {
                    //otro tipo de excepcion
                }
            }
            this.logs.Log(
                    name: "Renderizado de Marcas",
                    total_loads: this.total_loads,
                    total_errors: this.total_errors,
                    total_not_procecced: this.total_not_procecced,
                    json_details: JsonSerializer.Serialize(this.details.ToArray())
                ) ;
            
            this.console.color(ConsoleColor.Yellow).write("Fin de ejecución del cron").dot(2);
        }
    }
}
