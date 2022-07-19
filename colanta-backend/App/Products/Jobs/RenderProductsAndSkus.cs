namespace colanta_backend.App.Products.Jobs
{
    using System.Threading.Tasks;
    using Products.Application;
    using Products.Domain;
    using System;
    using System.Collections.Generic;
    using Shared.Application;
    using Shared.Domain;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public class RenderProductsAndSkus
    {
        private string processName = "Renderizado de productos";
        private ProductsRepository productsLocalRepository;
        private SkusRepository skusLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusVtexRepository skusVtexRepository;
        private ProductsSiesaRepository siesaRepository;
        private IProcess processLogger;
        private ILogger logger;
        private CustomConsole console = new CustomConsole();
        private RenderProductsAndSkusMail renderProductsMail;

        private List<Sku> loadSkus;
        private List<Sku> failedSkus;
        private List<Sku> inactiveSkus;
        private List<Sku> inactivatedSkus;
        private List<Sku> notProccecedSkus;
        private int obtainedSkus;

        private List<Detail> details;
        private JsonSerializerOptions jsonOptions;
        public RenderProductsAndSkus
        (
            ProductsRepository productsLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusRepository skusLocalRepository,
            SkusVtexRepository skusVtexRepository,
            ProductsSiesaRepository siesaRepository,
            IProcess processLogger,
            ILogger logger,
            EmailSender emailSender
        )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusVtexRepository = skusVtexRepository;
            this.siesaRepository = siesaRepository;
            this.processLogger = processLogger;
            this.logger = logger;
            this.renderProductsMail = new RenderProductsAndSkusMail(emailSender);

            this.loadSkus = new List<Sku>();
            this.failedSkus = new List<Sku>();
            this.inactiveSkus = new List<Sku>();
            this.inactivatedSkus = new List<Sku>();
            this.notProccecedSkus = new List<Sku>();

            this.details = new List<Detail>();
            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            this.jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

        }

        public async Task Invoke()
        {
            try
            {
                this.console.processStartsAt(processName, DateTime.Now);

                Product[] allSiesaProducts = await siesaRepository.getAllProducts();
                Sku[] allSiesaSkus = await siesaRepository.getAllSkus();
                this.obtainedSkus = allSiesaSkus.Length;
                this.details.Add(new Detail("siesa", "obtener todos los skus", JsonSerializer.Serialize(allSiesaSkus, jsonOptions), null, true));

                Product[] deltaProducts = await productsLocalRepository.getDeltaProducts(allSiesaProducts);
                Sku[] deltaSkus = await skusLocalRepository.getDeltaSkus(allSiesaSkus);

                foreach (Product deltaProduct in deltaProducts)
                {
                    try
                    {
                        deltaProduct.is_active = false;
                        await this.productsVtexRepository.updateProduct(deltaProduct);
                    }
                    catch (VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.logger.writelog(vtexException);
                    }
                }
                await productsLocalRepository.updateProducts(deltaProducts);

                foreach (Sku deltaSku in deltaSkus)
                {
                    try
                    {
                        deltaSku.is_active = false;
                        await skusVtexRepository.updateSku(deltaSku);
                        this.inactivatedSkus.Add(deltaSku);
                        this.details.Add(new Detail(
                                origin: "vtex",
                                action: "actualizar estado en vtex",
                                content: null,
                                description: null,
                                success: true
                            ));
                    }
                    catch (VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
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
                await skusLocalRepository.updateSkus(deltaSkus);

                foreach (Product siesaProduct in allSiesaProducts)
                {
                    Product? localProduct = await productsLocalRepository.getProductBySiesaId(siesaProduct.siesa_id);

                    if (localProduct != null && localProduct.is_active == false)
                    {
                        //hay que activar
                    }

                    if (localProduct != null && localProduct.is_active == true)
                    {
                        //todo ok
                    }

                    if (localProduct == null)
                    {
                        try
                        {
                            localProduct = await productsLocalRepository.saveProduct(siesaProduct);
                            Product vtexProduct = await productsVtexRepository.saveProduct(localProduct);
                            localProduct.vtex_id = vtexProduct.vtex_id;
                            await productsLocalRepository.updateProduct(localProduct);
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.logger.writelog(vtexException);
                        }
                        catch (Exception exception)
                        {
                            this.console.throwException(exception.Message);
                            this.logger.writelog(exception);
                        }
                    }
                }

                foreach (Sku siesaSku in allSiesaSkus)
                {
                    Sku? localSku = await skusLocalRepository.getSkuBySiesaId(siesaSku.siesa_id);

                    if (localSku != null && localSku.is_active == false)
                    {
                        this.inactiveSkus.Add(localSku);
                    }

                    if (localSku != null && localSku.is_active == true)
                    {
                        this.notProccecedSkus.Add(localSku);
                    }

                    if (localSku == null)
                    {
                        try
                        {
                            localSku = await skusLocalRepository.saveSku(siesaSku);
                            Sku vtexSku = await skusVtexRepository.saveSku(localSku);
                            localSku.vtex_id = vtexSku.vtex_id;
                            await skusLocalRepository.updateSku(localSku);

                            this.loadSkus.Add(localSku);
                            this.details.Add(new Detail(
                                origin: "vtex",
                                action: "crear sku",
                                content: null,
                                description: null,
                                success: true
                            ));
                        }
                        catch (VtexException vtexException)
                        {
                            this.console.throwException(vtexException.Message);
                            this.failedSkus.Add(siesaSku);
                            this.details.Add(new Detail(
                                origin: "vtex",
                                action: vtexException.requestUrl,
                                content: vtexException.responseBody,
                                description: vtexException.Message,
                                success: false
                            ));
                            this.logger.writelog(vtexException);
                        }
                        catch (Exception exception)
                        {
                            this.console.throwException(exception.Message);
                            this.logger.writelog(exception);
                        }
                    }
                }
                this.processLogger.Log(
                        name: this.processName,
                        this.loadSkus.Count,
                        this.failedSkus.Count,
                        this.notProccecedSkus.Count + this.inactiveSkus.Count,
                        this.obtainedSkus,
                        JsonSerializer.Serialize(this.details, jsonOptions)
                    );

                this.console.processEndstAt(processName, DateTime.Now);
            }
            catch(SiesaException exception)
            {
                this.console.throwException(exception.Message);
                this.details.Add(new Detail("siesa", exception.requestUrl, exception.responseBody, exception.Message, false));
                this.processLogger.Log(
                        name: this.processName,
                        this.loadSkus.Count,
                        this.failedSkus.Count,
                        this.notProccecedSkus.Count + this.inactiveSkus.Count,
                        this.obtainedSkus,
                        JsonSerializer.Serialize(this.details, jsonOptions)
                    );
                this.logger.writelog(exception);
                this.console.processEndstAt(processName, DateTime.Now);
            }
            catch(Exception exception)
            {
                this.console.throwException(exception.Message);
                this.processLogger.Log(
                        name: this.processName,
                        this.loadSkus.Count,
                        this.failedSkus.Count,
                        this.notProccecedSkus.Count + this.inactiveSkus.Count,
                        this.obtainedSkus,
                        JsonSerializer.Serialize(this.details, jsonOptions)
                    );
                this.logger.writelog(exception);
                this.console.processEndstAt(processName, DateTime.Now);
            }
        }
    }
}
