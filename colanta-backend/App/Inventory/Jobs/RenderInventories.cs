namespace colanta_backend.App.Inventory.Jobs
{
    using App.Inventory.Domain;
    using System.Threading.Tasks;
    using System;
    public class RenderInventories
    {
        private InventoriesRepository localRepository;
        private InventoriesVtexRepository vtexRepository;
        private InventoriesSiesaRepository siesaRepository;
        private WarehousesRepository warehousesRepository;
        public RenderInventories(
            InventoriesRepository localRepository, 
            InventoriesVtexRepository vtexRepository, 
            InventoriesSiesaRepository siesaRepository,
            WarehousesRepository warehousesRepository
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.warehousesRepository = warehousesRepository;
        }

        public async Task Invoke()
        {
            Warehouse[] allWarehouses = await this.warehousesRepository.getAllWarehouses();

            foreach(Warehouse warehouse in allWarehouses)
            {
                Inventory[] siesaInventories = await this.siesaRepository.getAllInventoriesByWarehouse(warehouse.siesa_id);
                foreach(Inventory siesaInventory in siesaInventories)
                {
                    Inventory localInventory = await this.localRepository.getInventoryByConcatSiesaIdAndWarehouseSiesaId(siesaInventory.sku_concat_siesa_id, siesaInventory.warehouse_siesa_id);
                    if(localInventory == null)
                    {
                        localInventory = await this.localRepository.saveInventory(siesaInventory);
                        await this.vtexRepository.updateInventory(localInventory);
                    }
                    if(localInventory != null)
                    {
                        if(localInventory.quantity != siesaInventory.quantity)
                        {
                            localInventory.quantity = siesaInventory.quantity;
                            localInventory = await this.localRepository.updateInventory(localInventory);
                            await this.vtexRepository.updateInventory(localInventory);
                        }
                    }
                }
            }
        }
    }
}
