using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Products.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using Products.Domain;
    public class RenderProductsMailModel : PageModel
    {
        public string title;
        public List<Sku> loadedSkus;
        public List<Sku> inactivatedSkus;
        public List<Sku> failedSkus;
        public DateTime dateTime;

        public RenderProductsMailModel(List<Sku> loadedSkus, List<Sku> inactivatedSkus, List<Sku> failedSkus)
        {
            this.title = "Renderizado de productos";
            this.loadedSkus = loadedSkus;
            this.inactivatedSkus = inactivatedSkus;
            this.failedSkus = failedSkus;
            this.dateTime = DateTime.Now;
        }

        public void OnGet()
        {
        }
    }
}
