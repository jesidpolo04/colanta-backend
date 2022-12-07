using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Prices.Infraestructure
{
    using Products.Domain;
    public class NotifyMissingPricesMailModel : PageModel
    {
        public Sku sku;

        public NotifyMissingPricesMailModel(Sku sku)
        {
            this.sku = sku;
        }
        public void OnGet()
        {
        }
    }
}
