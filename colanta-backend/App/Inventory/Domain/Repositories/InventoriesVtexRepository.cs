namespace colanta_backend.App.Inventory.Domain
{
    using System.Threading.Tasks;
    public interface InventoriesVtexRepository
    {
        void changeEnvironment(string environment);
        Task<Inventory> updateInventory(Inventory inventory);
        Task removeReservedInventory(Inventory inventory);
    }
}
