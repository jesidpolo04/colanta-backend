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
    using Microsoft.Extensions.Configuration;

    public class RenderBrands
    {
        private BrandsRepository brandsLocalRepository;
        private BrandsVtexRepository brandsVtexRepository;
        private ILogs logs;
        private EmailSender emailSender;

        private CustomConsole console;
        
        private List<Detail> details;
        private List<Brand> loadBrands;
        private List<Brand> inactiveBrands;
        private List<Brand> failedLoadBrands;
        

        private int total_loads = 0;
        private int total_errors = 0;
        private int total_not_procecced = 0;

        public RenderBrands(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository, ILogs logs)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
            this.logs = logs;

            this.emailSender = new GmailSender();
            this.console = new CustomConsole();
            this.details = new List<Detail>();

            this.inactiveBrands = new List<Brand>();
            this.loadBrands = new List<Brand>();
            this.failedLoadBrands = new List<Brand>();

            this.console.color(ConsoleColor.Cyan).write("Inicio de ejecución del cron:").color(ConsoleColor.Yellow).write("" + DateTime.Now).dot();
        }

        public async void Invoke()
        {
            BrandsSiesaRepository brandsSiesaRepository = new BrandsSiesaRepository();
            GetDeltaBrands getDeltaBrands = new GetDeltaBrands(this.brandsLocalRepository);
            GetBrandBySiesaId getBrandBySiesaId = new GetBrandBySiesaId(this.brandsLocalRepository);
            CreateBrand createBrand = new CreateBrand(this.brandsLocalRepository, this.logs);
            CreateVtexBrand createVtexBrand = new CreateVtexBrand(this.brandsVtexRepository);
            UpdateVtexBrand updateVtexBrand = new UpdateVtexBrand(this.brandsVtexRepository);
            UpdateBrands updateBrands = new UpdateBrands(this.brandsLocalRepository);
            UpdateBrand updateBrand = new UpdateBrand(this.brandsLocalRepository);

            this.console.color(ConsoleColor.Yellow).write("Consultado el endpoint de marcas de siesa, estado:");
            Brand[] siesaBrands = await brandsSiesaRepository.getAllBrands();
            this.details.Add(new Detail(
                    origin: "siesa",
                    success: true,
                    description: "Petición para obtener todas las marcas de siesa, completada con éxito"
                ));
            this.console.color(ConsoleColor.Green).write("completado con éxito")
                .color(ConsoleColor.Yellow).write("marcas traídas:")
                .color(ConsoleColor.Cyan).write(siesaBrands.Length.ToString()).dot();


            Brand[] deltaBrands = getDeltaBrands.Invoke(siesaBrands);
            if (deltaBrands.Length > 0)
            {
                this.console.color(ConsoleColor.Yellow).write("Inactivando marcas, cantidad a desactivar:")
                    .color(ConsoleColor.Red).write("" + deltaBrands.Length).dot();
                foreach (Brand deltaBrand in deltaBrands)
                {
                    deltaBrand.state = false;
                    this.console.color(ConsoleColor.Red).write("Inactivando:")
                        .color(ConsoleColor.Cyan).write(deltaBrand.name)
                        .color(ConsoleColor.Gray).write("( Siesa Id:")
                        .color(ConsoleColor.Cyan).write(deltaBrand.id_siesa.ToString())
                        .color(ConsoleColor.Gray).write(")").dot();

                }
                updateBrands.Invoke(deltaBrands);
                this.console.color(ConsoleColor.Yellow).write("Inactivación de marcas:")
                    .color(ConsoleColor.DarkGreen).write("completado con éxito").reset();
            }

            foreach (Brand siesaBrand in siesaBrands)
            {
                try
                {
                    Brand? localBrand = getBrandBySiesaId.Invoke(siesaBrand.id_siesa);

                    if (localBrand != null && localBrand.state == true)
                    {
                        this.total_not_procecced++;
                    }

                    if (localBrand != null && localBrand.state == false && localBrand.id_vtex != null)
                    {
                        this.inactiveBrands.Add(localBrand);
                        this.total_not_procecced++;
                    }

                    if (localBrand == null)
                    {
                        this.console.color(ConsoleColor.DarkGreen).write("Creando localmente:")
                            .color(ConsoleColor.Cyan).write(siesaBrand.name)
                            .color(ConsoleColor.Gray).write("( Siesa Id:")
                            .color(ConsoleColor.Cyan).write(siesaBrand.id_siesa.ToString())
                            .color(ConsoleColor.Gray).write("),")
                            .color(ConsoleColor.Yellow).write("estado");

                        localBrand = createBrand.Invoke(siesaBrand);

                        this.console.color(ConsoleColor.DarkMagenta).write("Creando en vtex:")
                           .color(ConsoleColor.Cyan).write(siesaBrand.name)
                           .color(ConsoleColor.Gray).write("( Siesa Id:")
                           .color(ConsoleColor.Cyan).write(siesaBrand.id_siesa.ToString())
                           .color(ConsoleColor.Gray).write("),")
                           .color(ConsoleColor.Yellow).write("estado");

                        Brand? vtexBrand = await createVtexBrand.Invoke(localBrand);


                        this.details.Add(new Detail(
                                                origin: "vtex",
                                                success: true,
                                                description: "request for create a brand in VTEX, completed successfully"
                                            ));
                        localBrand.id_vtex = vtexBrand.id_vtex;
                        updateBrand.Invoke(localBrand);
                        this.loadBrands.Add(vtexBrand);
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
                    this.failedLoadBrands.Add(siesaBrand);
                    this.total_errors++;
                    //this.emailSender.SendEmail("¡Alerta!, no fue posible crear en vtex la marca: " + siesaBrand.name);
                }
                catch (Exception exception)
                {
                    //normalmente problemas de conexión del middleware
                    this.console.color(ConsoleColor.Red).write("Hubo un problema:")
                        .color(ConsoleColor.Gray).write(exception.Message).dot();
                }
            }

            this.logs.Log(
                    name: "Renderizado de Marcas",
                    total_loads: this.total_loads,
                    total_errors: this.total_errors,
                    total_not_procecced: this.total_not_procecced,
                    json_details: JsonSerializer.Serialize(this.details.ToArray())
                );

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

            this.emailSender.SendEmail(emailMessage);

            this.console.color(ConsoleColor.Cyan).write("Fin de ejecución del cron").color(ConsoleColor.Yellow).write("" + DateTime.Now).dot(2);
        }
    }
}
