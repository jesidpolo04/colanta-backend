namespace colanta_backend.App.Inventory.Jobs
{
    using App.Inventory.Domain;
    using Products.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using App.Shared.Domain;
    using App.Shared.Application;
    using System.Linq;
    
    public class RenderInventories : IDisposable
    {
        private string processName = "Renderizado de inventarios";
        private IServiceProvider serviceProvider;
        private WarehousesRepository warehousesRepository;
        private IProcess process;
        private readonly ILogger logger;
        private readonly List<Inventory> loadInventories = new List<Inventory>();
        private readonly List<Inventory> updatedInventories = new List<Inventory>();
        private readonly List<Inventory> failedInventories = new List<Inventory>();
        private readonly List<Inventory> notProccecedInventories = new List<Inventory>();

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        private CustomConsole console = new CustomConsole();
        public RenderInventories(
            IServiceProvider serviceProvider,
            WarehousesRepository warehousesRepository,
            IProcess process,
            ILogger logger,
            IRenderInventoriesMail mail
            )
        {
            this.serviceProvider = serviceProvider;
            this.warehousesRepository = warehousesRepository;
            this.process = process;
            this.logger = logger;
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            this.console.processStartsAt(processName, DateTime.Now);

            Warehouse[] allWarehouses = await this.warehousesRepository.getAllWarehouses();
            string[] registeredSkus = await this.GetListOfRegisteredSkus((ISkusRepository)this.serviceProvider.GetService(typeof(ISkusRepository)));

            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = 5 // Limita a N tareas concurrentes
            };
            Parallel.ForEach(allWarehouses, options, async warehouse =>
            {
                IInventoriesRepository inventoriesLocalRepository = (IInventoriesRepository)this.serviceProvider.GetService(typeof(IInventoriesRepository));
                InventoriesVtexRepository inventoriesVtexRepository = (InventoriesVtexRepository)this.serviceProvider.GetService(typeof(InventoriesVtexRepository));
                InventoriesSiesaRepository inventoriesSiesaRepository = (InventoriesSiesaRepository)this.serviceProvider.GetService(typeof(InventoriesSiesaRepository));
                ILogger logger = (ILogger)this.serviceProvider.GetService(typeof(ILogger));

                try
                {
                    var siesaInventoriesTask = inventoriesSiesaRepository.getAllInventoriesByWarehouse(warehouse.siesa_id);
                    var localInventoriesTask = inventoriesLocalRepository.GetInventoriesByWarehouseSiesaId(warehouse.siesa_id);


                    Inventory[] siesaInventories = await siesaInventoriesTask;
                    Inventory[] localInventories = await localInventoriesTask;

                    foreach (Inventory siesaInventory in siesaInventories)
                    {
                        try
                        {
                            siesaInventory.quantity = siesaInventory.quantity <= siesaInventory.security_stock ? 0 : siesaInventory.quantity - siesaInventory.security_stock;
                            if (!SkuExists(registeredSkus, siesaInventory.sku_concat_siesa_id))
                            {
                                notProccecedInventories.Add(siesaInventory);
                                continue;
                            }
                            Inventory? localInventory = localInventories.FirstOrDefault(
                                    localInventory => localInventory.sku_concat_siesa_id == siesaInventory.sku_concat_siesa_id
                                        && localInventory.warehouse_siesa_id == siesaInventory.warehouse_siesa_id
                                );

                            if (localInventory is not null)
                            {
                                if (localInventory.quantity != siesaInventory.quantity || localInventory.infinite != siesaInventory.infinite)
                                {
                                    localInventory.quantity = siesaInventory.quantity; //todo: funcion que actualize la entidad (no hacerlo linea por linea como aquí)
                                    localInventory.infinite = siesaInventory.infinite;
                                    localInventory.security_stock = siesaInventory.security_stock;

                                    localInventory = inventoriesLocalRepository.updateInventory(localInventory).Result;
                                    _ = inventoriesVtexRepository.updateInventory(localInventory);
                                    _ = inventoriesVtexRepository.removeReservedInventory(localInventory);
                                    this.updatedInventories.Add(localInventory);
                                }

                                if (localInventory.quantity == siesaInventory.quantity)
                                {
                                    _ = inventoriesVtexRepository.updateInventory(localInventory);
                                    _ = inventoriesVtexRepository.removeReservedInventory(localInventory);
                                    this.notProccecedInventories.Add(localInventory);
                                }
                            }
                            if (localInventory == null)
                            {
                                try
                                {
                                    localInventory = inventoriesLocalRepository.saveInventory(siesaInventory).Result;
                                }
                                catch (Exception exception)
                                {
                                    logger.writelog(new Exception($"No se pudo guardar el inventario {localInventory} en la tienda ${localInventory.warehouse_siesa_id}"));
                                    continue;
                                }
                                inventoriesVtexRepository.updateInventory(localInventory);
                                this.loadInventories.Add(localInventory);
                            }
                        }
                        catch (VtexException vtexException)
                        {
                            this.failedInventories.Add(siesaInventory);
                            this.console.throwException(vtexException.Message);
                            logger.writelog(vtexException);
                        }
                        catch (Exception exception)
                        {
                            this.failedInventories.Add(siesaInventory);
                            this.console.throwException(exception.Message);
                            logger.writelog(exception);
                        }
                    }
                }
                catch (SiesaException siesaException)
                {
                    this.console.throwException(siesaException.Message);
                    logger.writelog(siesaException);
                }
                catch (Exception genericException)
                {
                    this.console.throwException(genericException.Message);
                    logger.writelog(genericException);
                }
            });
            //this.mail.sendMail(this.loadInventories, this.updatedInventories, this.failedInventories);
            this.console.processEndstAt(processName, DateTime.Now);
        }

        private static bool SkuExists(string[] existingSkusConcatSiesaIds, string skuConcatSiesaId)
        {
            // Se utliza una busqueda sobre el arreglo de skus existentes
            // Ya que consultarlos uno a uno es muy costoso para la base de datos
            return existingSkusConcatSiesaIds.Contains(skuConcatSiesaId);
        }

        private async Task<string[]> GetListOfRegisteredSkus(ISkusRepository repository)
        {
            return await repository.getAllSkusConcatSiesaIds();
        }

        public void Dispose()
        {
            this.loadInventories.Clear();
            this.updatedInventories.Clear();
            this.failedInventories.Clear();
            this.notProccecedInventories.Clear();
        }
    }
}
