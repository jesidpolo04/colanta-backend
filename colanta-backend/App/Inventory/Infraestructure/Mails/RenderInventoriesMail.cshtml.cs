using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Inventory.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using Inventory.Domain;
    public class RenderInventoriesMailModel : PageModel
    {
        public DateTime dateTime;
        public List<Inventory> loadedInventories;
        public List<Inventory> updatedInventories;
        public List<Inventory> failedInventories;

        public RenderInventoriesMailModel(List<Inventory> loadedInventories, List<Inventory> updatedInventories, List<Inventory> failedInventories)
        {
            this.dateTime = DateTime.Now;
            this.loadedInventories = loadedInventories;
            this.updatedInventories = updatedInventories;
            this.failedInventories = failedInventories;
        }

        public void OnGet()
        {
        }
    }
}
