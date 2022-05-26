namespace colanta_backend.App.Inventory.Domain
{
    using System.Threading.Tasks;
    public interface InventoriesRepository
    {
        Task<Inventory> saveInventory(Inventory inventory);
        Task<Inventory> updateInventory(Inventory inventory);
        Task<Inventory[]> updateInventories(Inventory[] inventories);
        Task<Inventory> getInventoryByConcatSiesaIdAndWarehouseSiesaId(string concatSiesaId, string warehouseSiesaId);
    }
}
