namespace colanta_backend.App.Inventory.Domain
{
    using System.Threading.Tasks;
    public interface WarehousesSiesaVtexRepository
    {
        Task<Warehouse[]> getAllWarehouses();
    }
}
