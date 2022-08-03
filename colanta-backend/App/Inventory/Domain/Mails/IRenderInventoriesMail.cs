namespace colanta_backend.App.Inventory.Domain
{
    using System.Collections.Generic;
    public interface IRenderInventoriesMail
    {
        void sendMail(List<Inventory> loadedInventories, List<Inventory> updatedInventories, List<Inventory> failedInventories);

    }
}
