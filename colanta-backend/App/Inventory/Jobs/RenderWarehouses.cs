namespace colanta_backend.App.Inventory.Jobs
{
    using App.Inventory.Domain;
    using System.Threading.Tasks;
    using System;
    public class RenderWarehouses
    {
        private WarehousesSiesaVtexRepository siesaVtexRepository;
        private WarehousesRepository localRepository;

        public RenderWarehouses(WarehousesSiesaVtexRepository siesaVtexRepository, WarehousesRepository localRepository)
        {
            this.siesaVtexRepository = siesaVtexRepository;
            this.localRepository = localRepository;
        }

        public async Task Invoke()
        {
            Warehouse[] siesaWarehouses = await this.siesaVtexRepository.getAllWarehouses();
            foreach(Warehouse siesaWarehouse in siesaWarehouses)
            {
                Warehouse localWarehouse = await this.localRepository.getWarehouseBySiesaId(siesaWarehouse.siesa_id);
                
                if(localWarehouse == null)
                {
                    await this.localRepository.saveWarehouse(siesaWarehouse);
                }
            }
        }
    }
}
