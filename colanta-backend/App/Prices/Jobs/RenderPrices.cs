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
        public ILogs logs;

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
            ILogs logs)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.emailSender = emailSender;
            this.logs = logs;

            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            this.renderPricesMail = new RenderPricesMail(emailSender);
        }

        public async Task Invoke()
        {
            this.console.warningColor().write("Iniciando proceso:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

            try
            {
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
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.failedPrices.Add(siesaPrice);
                            this.details.Add(new Detail(
                                        origin: "vtex",
                                        action: "crear o actualizar precio",
                                        content: vtexException.Message,
                                        description: vtexException.Message,
                                        success: false
                                        ));
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
                                }
                                if (vtexPrice.price == localPrice.price)
                                {
                                    this.notProccecedPrices.Add(localPrice);
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.failedPrices.Add(siesaPrice);
                            this.details.Add(new Detail(
                                        origin: "vtex",
                                        action: "crear o actualizar precio",
                                        content: vtexException.Message,
                                        description: vtexException.Message,
                                        success: false
                                        ));
                        }
                    }
                }
            }
            catch(SiesaException siesaException)
            {
                this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: "obtener todos los precios",
                                    content: siesaException.Message,
                                    description: siesaException.Message,
                                    success: false
                                    ));
            }
            catch(Exception genericException)
            {

            }

            this.logs.Log(
                name: this.processName,
                total_loads: this.loadPrices.Count + this.updatedPrices.Count,
                total_errors: this.failedPrices.Count,
                total_not_procecced: this.notProccecedPrices.Count,
                total_obtained: this.obtainedPrices.Count,
                json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );

            this.renderPricesMail.sendMail(
                this.loadPrices.ToArray(),
                this.updatedPrices.ToArray(),
                this.failedPrices.ToArray()
                );

            this.writeConsoleLogs();
            this.console.warningColor().write("Proceso Finalizado:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();
        }

        private void writeConsoleLogs()
        {
            if (this.loadPrices.Count > 0)
            {
                this.console.errorColor().writeLine("Precios cargados a Vtex");
                foreach (Price loadPrice in this.loadPrices)
                {
                    this.console.whiteColor().write(loadPrice.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(loadPrice.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(loadPrice.sku.siesa_id.ToString())
                        .grayColor().write("precio:")
                        .infoColor().write(loadPrice.price.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.updatedPrices.Count > 0)
            {
                this.console.successColor().writeLine("Precios actualizados en Vtex");
                foreach (Price updatedPrice in this.updatedPrices)
                {
                    this.console.whiteColor().write(updatedPrice.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(updatedPrice.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(updatedPrice.sku.siesa_id.ToString())
                        .grayColor().write("nuevo precio:")
                        .infoColor().write(updatedPrice.price.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.failedPrices.Count > 0)
            {
                this.console.errorColor().writeLine("Precios que fallaron al intentar cargar en Vtex");
                foreach (Price failedPrice in this.failedPrices)
                {
                    this.console.whiteColor().write(failedPrice.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(failedPrice.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(failedPrice.sku.siesa_id.ToString())
                        .grayColor().write("precio:")
                        .infoColor().write(failedPrice.price.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.notProccecedPrices.Count > 0)
            {
                this.console.warningColor().writeLine("Precios que se mantienen");
                foreach (Price notProccecedPrice in this.notProccecedPrices)
                {
                    this.console.whiteColor().write(notProccecedPrice.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(notProccecedPrice.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(notProccecedPrice.sku.siesa_id.ToString())
                        .grayColor().write("precio:")
                        .infoColor().write(notProccecedPrice.price.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }
        }
    }
}
