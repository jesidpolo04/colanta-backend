namespace colanta_backend.App.Inventory.Domain
{
    using System.Threading.Tasks;
    public interface WarehousesRepository
    {
        Task<Warehouse> getWarehouseBySiesaId(string siesaId);
        Task<Warehouse> saveWarehouse(Warehouse warehouse);
        Task<Warehouse[]> getAllWarehouses();
        Task<Warehouse> updateWarehouse(Warehouse warehouse);
    }
}
