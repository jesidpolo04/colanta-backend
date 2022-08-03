using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    public class InvalidCategoryMailModel : PageModel
    {
        public Product product;

        public InvalidCategoryMailModel(Product product)
        {
            this.product = product;
        }
        public void OnGet()
        {
        }
    }
}
