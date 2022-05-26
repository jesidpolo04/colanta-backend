using colanta_backend.App.Inventory.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Inventory.Infraestructure
{
    using App.Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using System.Collections.Generic;
    public class WarehousesEFRepository : Domain.WarehousesRepository
    {
        private ColantaContext dbContext;
        
        public WarehousesEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<Warehouse[]> getAllWarehouses()
        {
            EFWarehouse[] efWarehouses = dbContext.Warehouses.ToArray();
            List<Warehouse> warehouses = new List<Warehouse>();
 
            foreach(EFWarehouse efWarehouse in efWarehouses)
            {
                warehouses.Add(efWarehouse.getWarehouseFromEfWarehouse());
            }

            return warehouses.ToArray();
        }

        public async Task<Warehouse?> getWarehouseBySiesaId(string siesaId)
        {
            var efWarehouses = this.dbContext.Warehouses.Where(warehouse => warehouse.siesa_id == siesaId);
            if(efWarehouses.ToArray().Length > 0)
            {
                return efWarehouses.First().getWarehouseFromEfWarehouse();
            }
            return null;
        }

        public async Task<Warehouse> saveWarehouse(Warehouse warehouse)
        {
            EFWarehouse efWarehouse = new EFWarehouse();
            efWarehouse.setEfWarehouseFromWarehouse(warehouse);
            this.dbContext.Add(efWarehouse);
            this.dbContext.SaveChanges();
            return await this.getWarehouseBySiesaId(warehouse.siesa_id);
        }

        public async Task<Warehouse> updateWarehouse(Warehouse warehouse)
        {
            EFWarehouse efWarehouse = this.dbContext.Warehouses.Find(warehouse.id);
            efWarehouse.siesa_id = warehouse.siesa_id;
            efWarehouse.vtex_id = warehouse.vtex_id;
            efWarehouse.name = warehouse.name;
            efWarehouse.business = warehouse.business;
            this.dbContext.SaveChanges();
            return warehouse;
        }
    }
}
