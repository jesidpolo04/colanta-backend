namespace colanta_backend.App.Inventory.Jobs
{
    using App.Inventory.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using App.Shared.Domain;
    using App.Shared.Application;
    public class RenderInventories
    {
        private string processName = "Renderizado de inventarios";
        private InventoriesRepository localRepository;
        private InventoriesVtexRepository vtexRepository;
        private InventoriesSiesaRepository siesaRepository;
        private WarehousesRepository warehousesRepository;
        private IProcess process;
        private ILogger logger;
        private EmailSender emailSender;

        private List<Inventory> loadInventories = new List<Inventory>();
        private List<Inventory> updatedInventories = new List<Inventory>();
        private List<Inventory> failedInventories = new List<Inventory>();
        private List<Inventory> notProccecedInventories = new List<Inventory>();
        private List<Inventory> obtainedInventories = new List<Inventory> ();

        private List<Detail> details = new List<Detail>();

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        private CustomConsole console = new CustomConsole();
        private RenderInventoriesMail renderInventoriesMail;
        public RenderInventories(
            InventoriesRepository localRepository, 
            InventoriesVtexRepository vtexRepository, 
            InventoriesSiesaRepository siesaRepository,
            WarehousesRepository warehousesRepository,
            IProcess process,
            ILogger logger,
            EmailSender emailSender
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.warehousesRepository = warehousesRepository;
            this.process = process;
            this.logger = logger;
            this.emailSender = emailSender;
            this.renderInventoriesMail = new RenderInventoriesMail(emailSender);

            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            this.console.processStartsAt(processName, DateTime.Now);

            Warehouse[] allWarehouses = await this.warehousesRepository.getAllWarehouses();

            foreach(Warehouse warehouse in allWarehouses)
            {
                try
                {
                    Inventory[] siesaInventories = await this.siesaRepository.getAllInventoriesByWarehouse(warehouse.siesa_id);
                    foreach(Inventory inventory in siesaInventories)
                    {
                        this.obtainedInventories.Add(inventory);
                    }
                    this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: "traer todos los inventarios",
                                    content: JsonSerializer.Serialize(siesaInventories, this.jsonOptions),
                                    description: "petición completada con éxito",
                                    success: true));
                    foreach (Inventory siesaInventory in siesaInventories)
                    {
                        try
                        {
                            Inventory localInventory = await this.localRepository.getInventoryByConcatSiesaIdAndWarehouseSiesaId(siesaInventory.sku_concat_siesa_id, siesaInventory.warehouse_siesa_id);
                            
                            if (localInventory != null)
                            {
                                if (localInventory.quantity != siesaInventory.quantity)
                                {
                                    localInventory.quantity = siesaInventory.quantity;
                                    localInventory = await this.localRepository.updateInventory(localInventory);
                                    await this.vtexRepository.updateInventory(localInventory);
                                    this.updatedInventories.Add(localInventory);

                                    this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: "crear o actualizar inventario",
                                    content: JsonSerializer.Serialize(localInventory, this.jsonOptions),
                                    description: "petición completada con éxito",
                                    success: true));
                                }

                                if (localInventory.quantity == siesaInventory.quantity)
                                {
                                    this.notProccecedInventories.Add(localInventory);
                                }
                            }
                            if (localInventory == null)
                            {
                                localInventory = await this.localRepository.saveInventory(siesaInventory);
                                await this.vtexRepository.updateInventory(localInventory);
                                this.loadInventories.Add(localInventory);

                                this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: "crear o actualizar inventario",
                                    content: JsonSerializer.Serialize(localInventory, this.jsonOptions),
                                    description: "petición completada con éxito",
                                    success: true));
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.failedInventories.Add(siesaInventory);
                            this.console.throwException(vtexException.Message);
                            this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: vtexException.requestUrl,
                                    content: vtexException.responseBody,
                                    description: vtexException.Message,
                                    success: false));
                            this.logger.writelog(vtexException);
                        }
                    }
                }
                catch (SiesaException siesaException)
                {
                    this.console.throwException(siesaException.Message);
                    this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: siesaException.requestUrl,
                                    content: siesaException.responseBody,
                                    description: siesaException.Message,
                                    success: false));
                    this.logger.writelog(siesaException);
                }
                catch (Exception genericException)
                {
                    this.console.throwException(genericException.Message);
                    this.logger.writelog(genericException);
                }
            }
            this.process.Log(
                name: this.processName, 
                this.loadInventories.Count + this.updatedInventories.Count, 
                this.failedInventories.Count, 
                this.notProccecedInventories.Count, 
                this.obtainedInventories.Count, 
                JsonSerializer.Serialize(this.details, jsonOptions));

            this.renderInventoriesMail.sendMail(this.failedInventories.ToArray(), this.loadInventories.ToArray(), this.updatedInventories.ToArray());
            this.console.processEndstAt(processName, DateTime.Now);
        }

    }
}
