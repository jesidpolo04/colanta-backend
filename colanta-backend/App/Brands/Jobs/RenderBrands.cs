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
        private IProcess processLogger;
        private ILogger logger;
        private IConfiguration configuration;
        private RenderBrandsMail renderBrandsMail;

        private CustomConsole console;
        
        private List<Detail> details;
        private List<Brand> loadBrands;
        private List<Brand> inactiveBrands;
        private List<Brand> failedLoadBrands;
        private List<Brand> notProccecedBrands;
        private List<Brand> inactivatedBrands;
        private int obtainedBrands;
        private JsonSerializerOptions jsonOptions;

        public RenderBrands(
            BrandsRepository brandsLocalRepository, 
            BrandsVtexRepository brandsVtexRepository, 
            IProcess processLogger,
            ILogger logger,
            EmailSender emailSender, 
            IConfiguration configuration)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
            this.configuration = configuration;
            this.processLogger = processLogger;
            this.logger = logger;
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
        }

        public async Task Invoke()
        {
            try
            {
                this.console.processStartsAt(processName, DateTime.Now);
                BrandsSiesaRepository brandsSiesaRepository = new BrandsSiesaRepository(this.configuration);

                Brand[] siesaBrands = await brandsSiesaRepository.getAllBrands();
                this.obtainedBrands = siesaBrands.Length;
                this.details.Add(new Detail(
                        origin: "siesa",
                        action: "obtener todas las marcas",
                        success: true,
                        description: null,
                        content: JsonSerializer.Serialize(siesaBrands)
                    ));
                Brand[] deltaBrands = this.brandsLocalRepository.getDeltaBrands(siesaBrands);
                if (deltaBrands.Length > 0)
                {
                    foreach (Brand deltaBrand in deltaBrands)
                    {
                        try
                        {
                            deltaBrand.state = false;
                            await this.brandsVtexRepository.updateBrand(deltaBrand);
                            this.inactivatedBrands.Add(deltaBrand);
                            this.details.Add(new Detail(
                                    origin: "vtex",
                                    success: true,
                                    action: "desactivar marca",
                                    description: null,
                                    content: null
                                )); 
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: vtexException.requestUrl,
                                    success: false,
                                    description: vtexException.Message,
                                    content: vtexException.responseBody
                                ));
                            await this.logger.writelog(vtexException);
                        }
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
                            Brand? vtexBrand = await this.brandsVtexRepository.saveBrand(localBrand);

                            this.details.Add(
                                new Detail(
                                    origin: "vtex",
                                    action: "guardar marca",
                                    success: true,
                                    description: null,
                                    content: JsonSerializer.Serialize(vtexBrand, jsonOptions)
                            ));

                            localBrand.id_vtex = vtexBrand.id_vtex;
                            this.brandsLocalRepository.updateBrand(localBrand);
                            this.loadBrands.Add(vtexBrand);
                        }

                    }
                    catch (VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.details.Add(new Detail(
                                                origin: "vtex",
                                                action: vtexException.requestUrl,
                                                success: false,
                                                description: vtexException.Message,
                                                content: vtexException.responseBody
                                            ));
                        this.failedLoadBrands.Add(siesaBrand);
                        await this.logger.writelog(vtexException);
                    }
                    catch (Exception exception)
                    {
                        this.console.throwException(exception.Message);
                        await this.logger.writelog(exception);
                    }
                }
                this.processLogger.Log(
                    name: this.processName,
                    total_loads: this.loadBrands.Count,
                    total_errors: this.failedLoadBrands.Count,
                    total_not_procecced: this.inactiveBrands.Count + notProccecedBrands.Count,
                    total_obtained: siesaBrands.Length,
                    json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );
                this.console.processEndstAt(processName, DateTime.Now);
            }
            catch (SiesaException siesaException)
            {
                this.console.throwException(siesaException.Message);
                this.details.Add(new Detail(
                    origin: "siesa",
                    action: siesaException.requestUrl,
                    content: siesaException.responseBody,
                    description: siesaException.Message,
                    success: false
                    ));
                this.processLogger.Log(
                    name: this.processName,
                    total_loads: this.loadBrands.Count,
                    total_errors: this.failedLoadBrands.Count,
                    total_not_procecced: this.inactiveBrands.Count + notProccecedBrands.Count,
                    total_obtained: obtainedBrands,
                    json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );
                await this.logger.writelog(siesaException);
                this.console.processEndstAt(processName, DateTime.Now);
            }
            catch (Exception exception)
            {
                this.console.throwException(exception.Message);
                this.processLogger.Log(
                    name: this.processName,
                    total_loads: this.loadBrands.Count,
                    total_errors: this.failedLoadBrands.Count,
                    total_not_procecced: this.inactiveBrands.Count + notProccecedBrands.Count,
                    total_obtained: obtainedBrands,
                    json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );
                await this.logger.writelog(exception);
                this.console.processEndstAt(processName, DateTime.Now);
            }

            this.renderBrandsMail.sendMail(inactiveBrands.ToArray(), failedLoadBrands.ToArray());
        }
    }
}
