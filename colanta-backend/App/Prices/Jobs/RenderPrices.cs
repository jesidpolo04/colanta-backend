namespace colanta_backend.App.Prices.Jobs
{
    using System.Threading.Tasks;
    using App.Prices.Domain;
    using App.Shared.Domain;
    using App.Shared.Application;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System;
    public class RenderPrices
    {
        public string processName = "Renderizado de precios";
        public PricesRepository localRepository; 
        public PricesVtexRepository vtexRepository ;
        public PricesSiesaRepository siesaRepository;
        public EmailSender emailSender;
        public IProcess processLogger;
        public ILogger logger;

        public List<Price> loadPrices = new List<Price>();
        public List<Price> updatedPrices = new List<Price>();
        public List<Price> failedPrices = new List<Price>();
        public List<Price> notProccecedPrices = new List<Price>();
        public List<Price> obtainedPrices = new List<Price>();
        public List<Detail> details = new List<Detail>();

        public CustomConsole console = new CustomConsole();

        public JsonSerializerOptions jsonOptions;
        public RenderPricesMail renderPricesMail;
        public RenderPrices(
            PricesRepository localRepository, 
            PricesVtexRepository vtexRepository, 
            PricesSiesaRepository siesaRepository,
            EmailSender emailSender,
            IProcess processLogger,
            ILogger logger)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.emailSender = emailSender;
            this.processLogger = processLogger;
            this.logger = logger;

            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            this.renderPricesMail = new RenderPricesMail(emailSender);
        }

        public async Task Invoke()
        {
            try
            {
                this.console.processStartsAt(processName, DateTime.Now);
                Price[] allSiesaPrices = await this.siesaRepository.getAllPrices();
                this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: "obtener todos los precios",
                                    content: JsonSerializer.Serialize(allSiesaPrices, this.jsonOptions),
                                    description: "petición de obtener todos los precios, completada con éxito.",
                                    success: true
                                    ));

                foreach (Price siesaPrice in allSiesaPrices)
                {
                    this.obtainedPrices.Add(siesaPrice);

                    Price localPrice = await this.localRepository.getPriceBySkuConcatSiesaId(siesaPrice.sku_concat_siesa_id);

                    if (localPrice == null)
                    {
                        try
                        {
                            localPrice = await this.localRepository.savePrice(siesaPrice);
                            Price vtexPrice = await this.vtexRepository.getPriceByVtexId(localPrice.sku.vtex_id);
                            if (vtexPrice == null)
                            {
                                await this.vtexRepository.savePrice(localPrice);
                                this.loadPrices.Add(localPrice);
                                this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: "crear o actualizar precio",
                                    content: JsonSerializer.Serialize(localPrice, this.jsonOptions),
                                    description: "petición de crear o actualizar precio, completada con éxito.",
                                    success: true
                                    ));
                                continue;
                            }
                            if (vtexPrice != null)
                            {
                                if (vtexPrice.price != localPrice.price)
                                {
                                    await this.vtexRepository.savePrice(localPrice);
                                    this.loadPrices.Add(localPrice);
                                    this.details.Add(new Detail(
                                        origin: "vtex",
                                        action: "crear o actualizar precio",
                                        content: JsonSerializer.Serialize(localPrice, this.jsonOptions),
                                        description: "petición de crear o actualizar precio, completada con éxito.",
                                        success: true
                                        ));
                                    continue;
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.failedPrices.Add(siesaPrice);
                            this.details.Add(new Detail(
                                        origin: "vtex",
                                        action: vtexException.requestUrl,
                                        content: vtexException.responseBody,
                                        description: vtexException.Message,
                                        success: false
                                        ));
                            this.logger.writelog(vtexException);
                        }
                    }

                    if (localPrice != null)
                    {
                        try
                        {
                            if (localPrice.price != siesaPrice.price)
                            {
                                localPrice.price = siesaPrice.price;
                                await this.localRepository.updatePrice(localPrice);
                            }

                            Price vtexPrice = await this.vtexRepository.getPriceByVtexId(localPrice.sku.vtex_id);
                            if (vtexPrice == null)
                            {
                                await this.vtexRepository.savePrice(localPrice);
                                this.loadPrices.Add(localPrice);
                                this.details.Add(new Detail(
                                            origin: "vtex",
                                            action: "crear o actualizar precio",
                                            content: JsonSerializer.Serialize(localPrice, this.jsonOptions),
                                            description: "petición de crear o actualizar precio, completada con éxito.",
                                            success: true
                                            ));
                                continue;
                            }
                            if (vtexPrice != null)
                            {
                                if (vtexPrice.price != localPrice.price)
                                {
                                    await this.vtexRepository.savePrice(localPrice);
                                    this.updatedPrices.Add(localPrice);
                                    this.details.Add(new Detail(
                                            origin: "vtex",
                                            action: "crear o actualizar precio",
                                            content: JsonSerializer.Serialize(localPrice, this.jsonOptions),
                                            description: "petición de crear o actualizar precio, completada con éxito.",
                                            success: true
                                            ));
                                    continue;
                                }
                                if (vtexPrice.price == localPrice.price)
                                {
                                    this.notProccecedPrices.Add(localPrice);
                                    continue;
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.failedPrices.Add(siesaPrice);
                            this.details.Add(new Detail(
                                        origin: "vtex",
                                        action: vtexException.requestUrl,
                                        content: vtexException.responseBody,
                                        description: vtexException.Message,
                                        success: false
                                        ));
                            this.logger.writelog(vtexException);
                        }
                    }
                }
            }
            catch(SiesaException siesaException)
            {
                this.console.throwException(siesaException.Message);
                this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: "obtener todos los precios",
                                    content: siesaException.Message,
                                    description: siesaException.Message,
                                    success: false
                                    ));
                this.logger.writelog(siesaException);
            }
            catch(Exception genericException)
            {
                this.console.throwException(genericException.Message);
                this.logger.writelog(genericException);
            }

            this.renderPricesMail.sendMail(this.failedPrices.ToArray(), this.loadPrices.ToArray(), this.updatedPrices.ToArray());
            this.console.processEndstAt(processName, DateTime.Now);

            this.processLogger.Log(
                name: this.processName,
                total_loads: this.loadPrices.Count + this.updatedPrices.Count,
                total_errors: this.failedPrices.Count,
                total_not_procecced: this.notProccecedPrices.Count,
                total_obtained: this.obtainedPrices.Count,
                json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );
        }
    }
}
