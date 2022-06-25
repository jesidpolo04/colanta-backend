namespace colanta_backend.App.Brands.Jobs
{
    using App.Brands.Domain;
    using App.Shared.Domain;
    using App.Brands.Infraestructure;
    using App.Shared.Application;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    public class RenderBrands
    {
        public string processName = "Renderizado de marcas";
        private BrandsRepository brandsLocalRepository;
        private BrandsVtexRepository brandsVtexRepository;
        private ILogs logs;
        private IConfiguration configuration;
        private RenderBrandsMail renderBrandsMail;

        private CustomConsole console;
        
        private List<Detail> details;
        private List<Brand> loadBrands;
        private List<Brand> inactiveBrands;
        private List<Brand> failedLoadBrands;
        private List<Brand> notProccecedBrands;
        private List<Brand> inactivatedBrands;
        private JsonSerializerOptions jsonOptions;

        public RenderBrands(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository, ILogs logs, EmailSender emailSender, IConfiguration configuration)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
            this.configuration = configuration;
            this.logs = logs;
            this.renderBrandsMail = new RenderBrandsMail(emailSender);

            this.console = new CustomConsole();
            this.details = new List<Detail>();
            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

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
            BrandsSiesaRepository brandsSiesaRepository = new BrandsSiesaRepository(this.configuration);

            Brand[] siesaBrands = await brandsSiesaRepository.getAllBrands();

            System.Console.WriteLine("LLegó hasta aquí");
            this.details.Add(new Detail(
                    origin: "siesa",
                    success: true,
                    description: "Petición para obtener todas las marcas de siesa, completada con éxito",
                    content: JsonSerializer.Serialize(siesaBrands)
                ));
            Brand[] deltaBrands = this.brandsLocalRepository.getDeltaBrands(siesaBrands);
            if (deltaBrands.Length > 0)
            {
                foreach (Brand deltaBrand in deltaBrands)
                {
                    deltaBrand.state = false;
                    //await this.brandsVtexRepository.updateBrand(deltaBrand);
                    this.inactivatedBrands.Add(deltaBrand);
                    this.details.Add(new Detail(
                            origin: "vtex",
                            success: true,
                            description: "inactivación de la marca, completada con éxito",
                            content: JsonSerializer.Serialize(deltaBrand, this.jsonOptions)
                        ));
                }
                this.brandsLocalRepository.updateBrands(deltaBrands);
            }

            foreach (Brand siesaBrand in siesaBrands)
            {
                try
                {
                    Brand? localBrand = this.brandsLocalRepository.getBrandBySiesaId(siesaBrand.id_siesa);

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
                        localBrand = this.brandsLocalRepository.saveBrand(siesaBrand);
                        //Brand? vtexBrand = await this.brandsVtexRepository.saveBrand(localBrand);

                        /*this.details.Add(
                            new Detail(
                                origin: "vtex",
                                success: true,
                                description: "Petición para crear la marca en vtex, completada con éxito",
                                content: JsonSerializer.Serialize(vtexBrand, jsonOptions)
                        ));

                        localBrand.id_vtex = vtexBrand.id_vtex;
                        this.brandsLocalRepository.updateBrand(localBrand);
                        this.loadBrands.Add(vtexBrand); */
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
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            this.logs.Log(
                    name: this.processName,
                    total_loads: this.loadBrands.Count,
                    total_errors: this.failedLoadBrands.Count,
                    total_not_procecced: this.inactiveBrands.Count + notProccecedBrands.Count,
                    total_obtained: siesaBrands.Length,
                    json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );

            this.writeConsoleLogs();
            this.renderBrandsMail.sendMail(
                        loadBrands.ToArray(),
                        inactiveBrands.ToArray(),
                        failedLoadBrands.ToArray(),
                        notProccecedBrands.ToArray(),
                        inactivatedBrands.ToArray()
                    );

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
