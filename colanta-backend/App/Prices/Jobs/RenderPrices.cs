namespace colanta_backend.App.Prices.Jobs
{
    using System.Threading.Tasks;
    using App.Prices.Domain;
    using App.Specifications.Domain;
    using App.Products.Domain;
    using App.Shared.Domain;
    using App.Shared.Application;
    using System.Collections.Generic;
    using System.Text.Json;
    using System;
    public class RenderPrices : IDisposable
    {
        public string processName = "Renderizado de precios";
        public PricesRepository localRepository; 
        public PricesVtexRepository vtexRepository ;
        public PricesSiesaRepository siesaRepository;
        public SpecificationsVtexRepository specificationsVtexRepository;
        public SkusRepository skusLocalRepository;
        public IProcess processLogger;
        public ILogger logger;
        private IRenderPricesMail mail;
        private PoundSkusService poundSkusService;

        public List<Price> loadPrices = new List<Price>();
        public List<Price> updatedPrices = new List<Price>();
        public List<Price> failedPrices = new List<Price>();
        public List<Price> notProccecedPrices = new List<Price>();
        public int obtainedPrices = 0;
        public List<Detail> details = new List<Detail>();

        public CustomConsole console = new CustomConsole();
        public JsonSerializerOptions jsonOptions;

        public RenderPrices(
            PricesRepository localRepository,
            PricesVtexRepository vtexRepository,
            PricesSiesaRepository siesaRepository,
            SpecificationsVtexRepository specificationsVtexRepository,
            SkusRepository skusLocalRepository,
            IProcess processLogger,
            ILogger logger,
            IRenderPricesMail mail)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.specificationsVtexRepository = specificationsVtexRepository;
            this.processLogger = processLogger;
            this.logger = logger;
            this.mail = mail;
            this.poundSkusService = new PoundSkusService(skusLocalRepository);

            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            try
            {
                this.console.processStartsAt(processName, DateTime.Now);
                Price[] allSiesaPrices = await this.siesaRepository.getAllPrices();

                foreach (Price siesaPrice in allSiesaPrices)
                {
                    this.obtainedPrices ++;

                    if (!this.skuExist(siesaPrice))
                    {
                        this.notProccecedPrices.Add(siesaPrice);
                        continue;
                    }

                    if (this.poundSkusService.isPoundSku( this.getSiesaIdFromConcatSiesaId(siesaPrice.sku_concat_siesa_id) ))
                    {
                        siesaPrice.price = siesaPrice.price / 2;
                    }

                    Price localPrice = this.localRepository.getPriceBySkuConcatSiesaId(siesaPrice.sku_concat_siesa_id).Result;

                    if (localPrice != null)
                    {
                        try
                        {
                            if (localPrice.price != siesaPrice.price)
                            {
                                localPrice.price = siesaPrice.price;
                                await this.localRepository.updatePrice(localPrice);
                            }

                            Price vtexPrice = this.vtexRepository.getPriceByVtexId(localPrice.sku.vtex_id).Result;

                            if (vtexPrice == null)
                            {
                                this.vtexRepository.savePrice(localPrice).Wait();
                                this.updatePriceForMeasurementUnit(localPrice.sku.product, localPrice.price);
                                this.loadPrices.Add(localPrice);
                                continue;
                            }
                            if (vtexPrice != null)
                            {
                                if (vtexPrice.price != localPrice.price)
                                {
                                    this.vtexRepository.savePrice(localPrice).Wait();
                                    this.updatePriceForMeasurementUnit(localPrice.sku.product, localPrice.price);
                                    this.updatedPrices.Add(localPrice);
                                    continue;
                                }
                                if (vtexPrice.price == localPrice.price)
                                {
                                    this.updatePriceForMeasurementUnit(localPrice.sku.product, localPrice.price);
                                    this.notProccecedPrices.Add(localPrice);
                                    continue;
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.failedPrices.Add(localPrice);
                            this.logger.writelog(vtexException);
                        }
                        catch (Exception exception)
                        {
                            this.failedPrices.Add(localPrice);
                            this.console.throwException(exception.Message);
                            this.logger.writelog(exception);
                        }
                    }

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
                                continue;
                            }
                            if (vtexPrice != null)
                            {
                                if (vtexPrice.price != localPrice.price)
                                {
                                    await this.vtexRepository.savePrice(localPrice);
                                    this.loadPrices.Add(localPrice);
                                    continue;
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.failedPrices.Add(localPrice);
                            this.logger.writelog(vtexException);
                        }
                        catch (Exception exception)
                        {
                            this.failedPrices.Add(localPrice);
                            this.console.throwException(exception.Message);
                            this.logger.writelog(exception);
                        }
                    }
                }
            }
            catch(SiesaException siesaException)
            {
                this.console.throwException(siesaException.Message);
                this.logger.writelog(siesaException);
            }
            catch(Exception genericException)
            {
                this.console.throwException(genericException.Message);
                this.logger.writelog(genericException);
            }

            this.console.processEndstAt(processName, DateTime.Now);

            this.processLogger.Log(
                name: this.processName,
                total_loads: this.loadPrices.Count + this.updatedPrices.Count,
                total_errors: this.failedPrices.Count,
                total_not_procecced: this.notProccecedPrices.Count,
                total_obtained: this.obtainedPrices,
                json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                );
            this.mail.sendMail(this.loadPrices, this.updatedPrices, this.failedPrices);
        }

        private void updatePriceForMeasurementUnit(Product product, decimal price)
        {
            decimal content;
            decimal pmu; // price for measurement unit
            if (product.business == Stores.AGROCOLANTA) return;
            if (product.vtex_id == null) return;
            var specifications = this.specificationsVtexRepository.getProductSpecifications((int)product.vtex_id).Result;
            foreach(Specification specification in specifications)
            {
                if(specification.Id == SpecificationsIds.CONTENT && specification.Value.Count > 0) 
                {
                    if (specification.Value[0] == "") return;
                    content = Convert.ToDecimal(specification.Value[0]);
                    pmu = price / content;

                    var pmuSpecificationValue = new List<string>();
                    pmuSpecificationValue.Add(pmu.ToString("#.##"));
                    var pmuSpecification = new Specification(SpecificationsIds.PRICE_FOR_MEASUREMENT_UNIT, pmuSpecificationValue);
                    this.specificationsVtexRepository.updateProductSpecification((int)product.vtex_id, pmuSpecification);
                    break;
                }
            }
        }

        private bool skuExist(Price price)
        {
            Task<Sku?> sku = this.skusLocalRepository.getSkuByConcatSiesaId(price.sku_concat_siesa_id);
            if (sku.Result == null) {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            this.loadPrices.Clear();
            this.updatedPrices.Clear();
            this.failedPrices.Clear();
            this.notProccecedPrices.Clear();
            this.details.Clear();
        }

        private string getSiesaIdFromConcatSiesaId(string concatSiesaId)
        {
            return concatSiesaId.Split("_")[2];
        }
    }
}
