using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Orders.Infraestructure
{
    public class NewOrderMailModel : PageModel
    {
        public string orderId;

        public NewOrderMailModel(string orderId)
        {
            this.orderId = orderId;
        }
        public void OnGet()
        {
        }
    }
}
