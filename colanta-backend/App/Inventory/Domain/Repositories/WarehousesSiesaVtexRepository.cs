namespace colanta_backend.App.Inventory.Domain.Repositories
{
    using System.Threading.Tasks;
    public interface WarehousesSiesaVtexRepository
    {
        Task<Warehouse[]> getAllWarehouses();
    }
}
