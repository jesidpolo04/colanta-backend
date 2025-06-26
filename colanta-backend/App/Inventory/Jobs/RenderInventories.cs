namespace colanta_backend.App.Inventory.Jobs
{
    using App.Inventory.Domain;
    using Products.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using App.Shared.Domain;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    public class RenderInventories : IDisposable
    {
        private IServiceProvider serviceProvider;
        private WarehousesRepository warehousesRepository;
        private readonly ILogger<RenderInventories> _logger;
        private readonly List<Inventory> loadInventories = new List<Inventory>();
        private readonly List<Inventory> updatedInventories = new List<Inventory>();
        private readonly List<Inventory> failedInventories = new List<Inventory>();
        private readonly List<Inventory> notProccecedInventories = new List<Inventory>();

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
        public RenderInventories(
            IServiceProvider serviceProvider,
            WarehousesRepository warehousesRepository,
            ILogger<RenderInventories> logger,
            IRenderInventoriesMail mail
            )
        {
            this.serviceProvider = serviceProvider;
            this.warehousesRepository = warehousesRepository;
            _logger = logger;
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            try
            {
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
                                        _logger.LogError(exception, "Error al guardar el inventario {InventoryId} del almacén {WarehouseId}", siesaInventory.id, warehouse.siesa_id);
                                        continue;
                                    }
                                    _ = inventoriesVtexRepository.updateInventory(localInventory);
                                    this.loadInventories.Add(localInventory);
                                }
                            }
                            catch (VtexException vtexException)
                            {
                                _logger.LogError(vtexException, "Error al procesar el inventario {InventoryId} del almacén {WarehouseId}", siesaInventory.id, warehouse.siesa_id);
                            }
                            catch (Exception exception)
                            {
                                _logger.LogError(exception, "Error al procesar el inventario {InventoryId} del almacén {WarehouseId}", siesaInventory.id, warehouse.siesa_id);
                            }
                        }
                    }
                    catch (SiesaException siesaException)
                    {
                        _logger.LogError(siesaException, "Error al obtener inventarios de Siesa para el almacén {WarehouseId}", warehouse.siesa_id);
                    }
                    catch (Exception genericException)
                    {
                        _logger.LogError(genericException, "Error inesperado al procesar el almacén {WarehouseId}", warehouse.siesa_id);
                    }
                });
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar el proceso de renderizado de inventarios");
                return;
                
            }
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
