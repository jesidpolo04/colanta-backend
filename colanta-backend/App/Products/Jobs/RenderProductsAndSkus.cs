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
        private ILogs logs;
        private CustomConsole console = new CustomConsole();
        private RenderProductsAndSkusMail renderProductsMail;

        private List<Sku> loadSkus;
        private List<Sku> failedSkus;
        private List<Sku> inactiveSkus;
        private List<Sku> inactivatedSkus;
        private List<Sku> notProccecedSkus;

        private List<Detail> details;
        private JsonSerializerOptions jsonOptions;
        public RenderProductsAndSkus
        (
            ProductsRepository productsLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusRepository skusLocalRepository,
            SkusVtexRepository skusVtexRepository,
            ProductsSiesaRepository siesaRepository,
            ILogs logs,
            EmailSender emailSender
        )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusVtexRepository = skusVtexRepository;
            this.siesaRepository = siesaRepository;
            this.logs = logs;
            this.renderProductsMail = new RenderProductsAndSkusMail(emailSender);

            this.loadSkus = new List<Sku>();
            this.failedSkus = new List<Sku>();
            this.inactiveSkus = new List<Sku>();
            this.inactivatedSkus = new List<Sku>();
            this.notProccecedSkus  = new List<Sku>();

            this.details = new List<Detail>();
            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

        }

        public async Task<bool> Invoke()
        {
            GetAllProductsFromSiesa getAllProductsFromSiesa = new GetAllProductsFromSiesa(this.siesaRepository);
            GetAllSkusFromSiesa getAllSkusFromSiesa = new GetAllSkusFromSiesa(this.siesaRepository);
            GetDeltaProducts getDeltaProducts = new GetDeltaProducts(this.productsLocalRepository);
            GetDeltaSkus getDeltaSkus = new GetDeltaSkus(this.skusLocalRepository);
            GetSkuBySiesaId getSkuBySiesaId = new GetSkuBySiesaId(this.skusLocalRepository);
            GetVtexSkuBySiesaId getVtexSkuBySiesaId = new GetVtexSkuBySiesaId(this.skusVtexRepository);
            GetProductBySiesaId getProductBySiesaId = new GetProductBySiesaId(this.productsLocalRepository);
            GetVtexProductBySiesaId getVtexProductBySiesaId = new GetVtexProductBySiesaId(this.productsVtexRepository);
            UpdateProduct updateProduct = new UpdateProduct(this.productsLocalRepository);
            UpdateProducts updateProducts = new UpdateProducts(this.productsLocalRepository);
            UpdateSku updateSku = new UpdateSku(this.skusLocalRepository);
            UpdateSkus updateSkus = new UpdateSkus(this.skusLocalRepository);
            UpdateVtexProduct updateVtexProduct = new UpdateVtexProduct(this.productsVtexRepository);
            UpdateVtexSku updateVtexSku = new UpdateVtexSku(this.skusVtexRepository);
            SaveProduct createProduct = new SaveProduct(this.productsLocalRepository);
            SaveSku createSku = new SaveSku(this.skusLocalRepository);
            SaveVtexProduct createVtexProduct = new SaveVtexProduct(this.productsVtexRepository);
            SaveVtexSku createVtexSku = new SaveVtexSku(this.skusVtexRepository);

            this.console.warningColor().write("Iniciando proceso:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

            Product[] allSiesaProducts = await getAllProductsFromSiesa.Invoke();
            Sku[] allSiesaSkus = await getAllSkusFromSiesa.Invoke();

            Product[] deltaProducts = await getDeltaProducts.Invoke(allSiesaProducts);
            Sku[] deltaSkus = await getDeltaSkus.Invoke(allSiesaSkus);

            foreach(Product deltaProduct in deltaProducts)
            {
                try
                {
                    deltaProduct.is_active = false;
                    /*await updateVtexProduct.Invoke(deltaProduct);*/
                }
                catch(VtexException vtexException)
                {

                }
            }
            await updateProducts.Invoke(deltaProducts);

            foreach(Sku deltaSku in deltaSkus)
            {
                try
                {
                    deltaSku.is_active = false;
                    /*await updateVtexSku.Invoke(deltaSku);
                    this.loadSkus.Add(deltaSku);
                    this.details.Add(new Detail(
                            origin: "vtex",
                            action: "actualizar estado en vtex",
                            content: JsonSerializer.Serialize(deltaSku, this.jsonOptions),
                            description: "sku actualizado con éxito",
                            success: true
                        ));*/
                }
                catch(VtexException vtexException)
                {
                    this.failedSkus.Add(deltaSku);
                    this.details.Add(new Detail(
                            origin: "vtex",
                            action: "actualizar estado en vtex",
                            content: vtexException.Message,
                            description: vtexException.Message,
                            success: false
                            ));
                }
                
            }
            await updateSkus.Invoke(deltaSkus);

            foreach (Product siesaProduct in allSiesaProducts)
            {
                Product? localProduct = await getProductBySiesaId.Invoke(siesaProduct.siesa_id);

                if (localProduct != null && localProduct.is_active == false)
                {
                    //hay que activar
                }

                if(localProduct != null && localProduct.is_active == true)
                {
                    //todo ok
                }

                if(localProduct == null)
                {
                    try
                    {
                        localProduct = await createProduct.Invoke(siesaProduct);
                        /*Product vtexProduct = await createVtexProduct.Invoke(localProduct);
                        localProduct.vtex_id = vtexProduct.vtex_id;
                        await updateProduct.Invoke(localProduct);*/
                    }
                    catch (VtexException vtexException)
                    {

                    }
                    catch (Exception exception)
                    {
                        
                    }
                }
            }

            foreach (Sku siesaSku in allSiesaSkus)
            {
                Sku? localSku = await getSkuBySiesaId.Invoke(siesaSku.siesa_id);

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
                        localSku = await createSku.Invoke(siesaSku);
                        /*Sku vtexSku = await createVtexSku.Invoke(localSku);
                        localSku.vtex_id = vtexSku.vtex_id;
                        await updateSku.Invoke(localSku);

                        this.loadSkus.Add(localSku);
                        this.details.Add(new Detail(
                            origin: "vtex",
                            action: "crear sku",
                            content: JsonSerializer.Serialize(localSku, this.jsonOptions),
                            description: "sku creado con éxito",
                            success: true
                        ));*/
                    }
                    catch(VtexException vtexException)
                    {
                        this.failedSkus.Add(siesaSku);
                        this.details.Add(new Detail(
                            origin: "vtex",
                            action: "crear sku",
                            content: vtexException.Message,
                            description: vtexException.Message,
                            success: false
                        ));
                    }
                    catch(Exception exception)
                    {
                        System.Console.WriteLine("Exception: " + exception.Message + ": Stack: " + exception.StackTrace);
                    }
                }
            }
            this.logs.Log(
                    name: this.processName, 
                    this.loadSkus.Count, 
                    this.failedSkus.Count, 
                    this.notProccecedSkus.Count + this.inactiveSkus.Count, 
                    allSiesaSkus.Length, 
                    JsonSerializer.Serialize(this.details, jsonOptions)
                );

            this.writeConsoleLogs();

            this.renderProductsMail.sendMail(
                this.loadSkus.ToArray(), 
                this.inactiveSkus.ToArray(), 
                this.failedSkus.ToArray(),
                this.notProccecedSkus.ToArray(), 
                this.inactivatedSkus.ToArray()
                );

            this.console.warningColor().write("Proceso Finalizado:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

            return true;
        }

        private void writeConsoleLogs()
        {
            if (this.inactivatedSkus.Count > 0)
            {
                this.console.errorColor().writeLine("Productos desactivados");
                foreach (Sku inactivatedSku in this.inactivatedSkus)
                {
                    this.console.whiteColor().write(inactivatedSku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(inactivatedSku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(inactivatedSku.siesa_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.loadSkus.Count > 0)
            {
                this.console.successColor().writeLine("Productos desactivados");
                foreach (Sku loadSku in this.loadSkus)
                {
                    this.console.whiteColor().write(loadSku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(loadSku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(loadSku.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.failedSkus.Count > 0)
            {
                this.console.errorColor().writeLine("Productos no cargados a Vtex debiado a error");
                foreach (Sku failedSku in this.failedSkus)
                {
                    this.console.whiteColor().write(failedSku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(failedSku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(failedSku.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.inactiveSkus.Count > 0)
            {
                this.console.warningColor().writeLine("Productos por activar en Vtex");
                foreach (Sku inactiveSku in this.inactiveSkus)
                {
                    this.console.whiteColor().write(inactiveSku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(inactiveSku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(inactiveSku.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.notProccecedSkus.Count > 0)
            {
                this.console.successColor().writeLine("Productos no procesados");
                foreach (Sku notProccecedSku in this.notProccecedSkus)
                {
                    this.console.whiteColor().write(notProccecedSku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(notProccecedSku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(notProccecedSku.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }
        }
    }
}
