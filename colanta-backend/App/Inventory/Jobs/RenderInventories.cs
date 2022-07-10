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
        private ILogs logs;

        private List<Inventory> loadInventories = new List<Inventory>();
        private List<Inventory> updatedInventories = new List<Inventory>();
        private List<Inventory> failedInventories = new List<Inventory>();
        private List<Inventory> notProccecedInventories = new List<Inventory>();
        private List<Inventory> obtainedInventories = new List<Inventory> ();

        private List<Detail> details = new List<Detail>();

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        private CustomConsole console = new CustomConsole();
        public RenderInventories(
            InventoriesRepository localRepository, 
            InventoriesVtexRepository vtexRepository, 
            InventoriesSiesaRepository siesaRepository,
            WarehousesRepository warehousesRepository,
            ILogs logs
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.warehousesRepository = warehousesRepository;
            this.logs = logs;

            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            this.console.warningColor().write("Iniciando proceso:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

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
                            if (localInventory == null)
                            {
                                localInventory = await this.localRepository.saveInventory(siesaInventory);
                                //await this.vtexRepository.updateInventory(localInventory);
                                //this.loadInventories.Add(localInventory);

                                //this.details.Add(new Detail(
                                //    origin: "vtex",
                                //    action: "crear o actualizar inventario",
                                //    content: JsonSerializer.Serialize(localInventory, this.jsonOptions),
                                //    description: "petición completada con éxito",
                                //    success: true));
                            }
                            if (localInventory != null)
                            {
                                if (localInventory.quantity != siesaInventory.quantity)
                                {
                                    localInventory.quantity = siesaInventory.quantity;
                                    localInventory = await this.localRepository.updateInventory(localInventory);
                                    //await this.vtexRepository.updateInventory(localInventory);
                                    //this.updatedInventories.Add(localInventory);

                                    //this.details.Add(new Detail(
                                    //origin: "vtex",
                                    //action: "crear o actualizar inventario",
                                    //content: JsonSerializer.Serialize(localInventory, this.jsonOptions),
                                    //description: "petición completada con éxito",
                                    //success: true));
                                }

                                if (localInventory.quantity == siesaInventory.quantity)
                                {
                                    this.notProccecedInventories.Add(localInventory);
                                }
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: "crear o actualizar inventario",
                                    content: vtexException.Message,
                                    description: vtexException.Message,
                                    success: false));
                        }
                    }
                }
                catch (SiesaException sieaException)
                {
                    this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: "traer todos los inventarios",
                                    content: sieaException.Message,
                                    description: sieaException.Message,
                                    success: false));
                }
                catch (Exception genericException)
                {

                }
            }
            this.logs.Log(
                name: this.processName, 
                this.loadInventories.Count + this.updatedInventories.Count, 
                this.failedInventories.Count, 
                this.notProccecedInventories.Count, 
                this.obtainedInventories.Count, 
                JsonSerializer.Serialize(this.details, jsonOptions));

            this.writeConsoleLogs();
            this.console.warningColor().write("Proceso Finalizado:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();
        }

        private void writeConsoleLogs()
        {
            if (this.loadInventories.Count > 0)
            {
                this.console.errorColor().writeLine("Inventarios cargados a Vtex");
                foreach (Inventory loadInventory in this.loadInventories)
                {
                    this.console.whiteColor().write(loadInventory.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(loadInventory.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(loadInventory.sku.siesa_id.ToString())
                        .grayColor().write("almacen:")
                        .infoColor().write(loadInventory.warehouse.siesa_id)
                        .grayColor().write("precio:")
                        .infoColor().write(loadInventory.quantity.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.updatedInventories.Count > 0)
            {
                this.console.successColor().writeLine("Inventarios actualizados en Vtex");
                foreach (Inventory updatedInventory in this.updatedInventories)
                {
                    this.console.whiteColor().write(updatedInventory.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(updatedInventory.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(updatedInventory.sku.siesa_id.ToString())
                        .grayColor().write("almacen:")
                        .infoColor().write(updatedInventory.warehouse.siesa_id)
                        .grayColor().write("precio:")
                        .infoColor().write(updatedInventory.quantity.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.failedInventories.Count > 0)
            {
                this.console.errorColor().writeLine("Inventarios que fallaron al intentar cargar en Vtex");
                foreach (Inventory failedInventory in this.failedInventories)
                {
                    this.console.whiteColor().write(failedInventory.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(failedInventory.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(failedInventory.sku.siesa_id.ToString())
                        .grayColor().write("almacen:")
                        .infoColor().write(failedInventory.warehouse.siesa_id)
                        .grayColor().write("precio:")
                        .infoColor().write(failedInventory.quantity.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.notProccecedInventories.Count > 0)
            {
                this.console.warningColor().writeLine("Inventarios que se mantienen");
                foreach (Inventory notProccecedInventory in this.notProccecedInventories)
                {
                    this.console.whiteColor().write(notProccecedInventory.sku.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(notProccecedInventory.sku.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(notProccecedInventory.sku.siesa_id.ToString())
                        .grayColor().write("almacen:")
                        .infoColor().write(notProccecedInventory.warehouse.siesa_id)
                        .grayColor().write("precio:")
                        .infoColor().write(notProccecedInventory.quantity.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }
        }
    }
}
