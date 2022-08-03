using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Prices.Infraestructure
{
    using Prices.Domain;
    using System;
    using System.Collections.Generic;
    public class RenderPricesMailModel : PageModel
    {
        public List<Price> loadedPrices;
        public List<Price> updatedPrices;
        public List<Price> failedPrices;
        public DateTime dateTime;

        public RenderPricesMailModel(List<Price> loadedPrices, List<Price> updatedPrices, List<Price> failedPrices)
        {
            this.loadedPrices = loadedPrices;
            this.updatedPrices = updatedPrices;
            this.failedPrices = failedPrices;
            this.dateTime = DateTime.Now;
        }

        public void OnGet()
        {
        }
    }
}
