namespace colanta_backend.App.Inventory.Infraestructure
{
    using Microsoft.Extensions.Configuration;
    using App.Shared.Infraestructure;
    using System.Threading.Tasks;
    using colanta_backend.App.Inventory.Domain;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using App.Products.Infraestructure;

    public class InventoriesEFRepository : InventoriesRepository
    {
        private ColantaContext dbContext;

        public InventoriesEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<Inventory> getInventoryByConcatSiesaIdAndWarehouseSiesaId(string concatSiesaId, string warehouseSiesaId)
        {
            var efInventories = this.dbContext.Inventories
                .Include(inventory => inventory.warehouse)
                .Include(inventory => inventory.sku)
                .Where(inventory => inventory.sku_concat_siesa_id == concatSiesaId && inventory.warehouse_siesa_id == warehouseSiesaId);
            if(efInventories.ToArray().Length > 0)
            {
                EFInventory efInventory = efInventories.First();
                return efInventory.getInventoryFromEfInventory();
            }
            return null;   
        }

        public async Task<Inventory> saveInventory(Inventory inventory)
        {
            EFInventory efInventory = new EFInventory();
            efInventory.setEfInventoryFromInventory(inventory);

            EFWarehouse efWarehouse = this.dbContext.Warehouses.Where(warehouse => warehouse.siesa_id == inventory.warehouse_siesa_id).First();
            efInventory.warehouse = efWarehouse;

            EFSku efSku = this.dbContext.Skus.Where(sku => sku.concat_siesa_id == inventory.sku_concat_siesa_id).First();
            efInventory.sku = efSku;

            this.dbContext.Add(efInventory);
            this.dbContext.SaveChanges();
            return await this.getInventoryByConcatSiesaIdAndWarehouseSiesaId(inventory.sku_concat_siesa_id, inventory.warehouse_siesa_id);
        }

        public async Task<Inventory[]> updateInventories(Inventory[] inventories)
        {
            foreach(Inventory inventory in inventories)
            {
                EFInventory efInventory = this.dbContext.Inventories.Find(inventory.id);
                efInventory.quantity = inventory.quantity;
                efInventory.business = inventory.business;
                efInventory.sku_concat_siesa_id = inventory.sku_concat_siesa_id;
                efInventory.warehouse_siesa_id = inventory.warehouse_siesa_id;
            }
            this.dbContext.SaveChanges();
            return inventories;
        }

        public async Task<Inventory> updateInventory(Inventory inventory)
        {
            EFInventory efInventory = this.dbContext.Inventories.Find(inventory.id);
            efInventory.quantity = inventory.quantity;
            efInventory.business = inventory.business;
            efInventory.sku_concat_siesa_id = inventory.sku_concat_siesa_id;
            efInventory.warehouse_siesa_id = inventory.warehouse_siesa_id;
            this.dbContext.SaveChanges();
            return inventory;
        }
    }
}
