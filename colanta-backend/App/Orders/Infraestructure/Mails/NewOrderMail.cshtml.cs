using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    public class NewOrderMailModel : PageModel
    {
        public SiesaOrder siesaOrder;

        public NewOrderMailModel(SiesaOrder siesaOrder)
        {
            this.siesaOrder = siesaOrder;
        }
        public void OnGet()
        {
        }
    }
}
