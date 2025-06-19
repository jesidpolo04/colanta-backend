namespace colanta_backend.App.Inventory.Infraestructure
{
    using Microsoft.Extensions.Configuration;
    using App.Shared.Infraestructure;
    using System.Threading.Tasks;
    using colanta_backend.App.Inventory.Domain;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using App.Products.Infraestructure;

    public class InventoriesEFRepository : IInventoriesRepository
    {
        private ColantaContext _dbContext;
        private readonly IConfiguration _configuration;

        public InventoriesEFRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = new ColantaContext(configuration);
        }

        public Task<Inventory[]> GetInventoriesByWarehouseSiesaId(string warehouseSiesaId)
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                var efInventories = dbContext.Inventories
                    .Where(inventory => inventory.warehouse_siesa_id == warehouseSiesaId)
                    .ToArray();
                Inventory[] inventories = efInventories.Select(efInventory => efInventory.getInventoryFromEfInventory()).ToArray();
                return Task.FromResult(inventories);
            }
        }

        public async Task<Inventory> getInventoryByConcatSiesaIdAndWarehouseSiesaId(string concatSiesaId, string warehouseSiesaId)
        {
            var efInventories = this._dbContext.Inventories
                .Include(inventory => inventory.warehouse)
                .Include(inventory => inventory.sku)
                .Where(inventory => inventory.sku_concat_siesa_id == concatSiesaId && inventory.warehouse_siesa_id == warehouseSiesaId);
            if (efInventories.ToArray().Length > 0)
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

            EFWarehouse efWarehouse = this._dbContext.Warehouses.Where(warehouse => warehouse.siesa_id == inventory.warehouse_siesa_id).First();
            efInventory.warehouse = efWarehouse;

            EFSku efSku = this._dbContext.Skus.Where(sku => sku.concat_siesa_id == inventory.sku_concat_siesa_id).First();
            efInventory.sku = efSku;

            this._dbContext.Add(efInventory);
            this._dbContext.SaveChanges();
            return await this.getInventoryByConcatSiesaIdAndWarehouseSiesaId(inventory.sku_concat_siesa_id, inventory.warehouse_siesa_id);
        }

        public async Task<Inventory[]> updateInventories(Inventory[] inventories)
        {
            foreach (Inventory inventory in inventories)
            {
                EFInventory efInventory = this._dbContext.Inventories.Find(inventory.id);
                efInventory.quantity = inventory.quantity;
                efInventory.business = inventory.business;
                efInventory.sku_concat_siesa_id = inventory.sku_concat_siesa_id;
                efInventory.warehouse_siesa_id = inventory.warehouse_siesa_id;
                efInventory.infinite = inventory.infinite; //todo: hacer función en la entidad de infraestructura para actualizar
                efInventory.security_stock = inventory.security_stock;
            }
            this._dbContext.SaveChanges();
            return inventories;
        }

        public async Task<Inventory> updateInventory(Inventory inventory)
        {
            EFInventory efInventory = this._dbContext.Inventories.Find(inventory.id);
            efInventory.quantity = inventory.quantity;
            efInventory.business = inventory.business;
            efInventory.sku_concat_siesa_id = inventory.sku_concat_siesa_id;
            efInventory.warehouse_siesa_id = inventory.warehouse_siesa_id;
            efInventory.infinite = inventory.infinite;
            efInventory.security_stock = inventory.security_stock;
            this._dbContext.SaveChanges();
            return inventory;
        }
    }
}
