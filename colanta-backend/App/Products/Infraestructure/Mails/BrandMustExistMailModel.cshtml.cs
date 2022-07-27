using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    public class BrandMustExistMailModel : PageModel
    {
        public Product product { get; set; }

        public BrandMustExistMailModel(Product product)
        {
            this.product = product;
        }
        public void OnGet(Product product)
        {
            this.product = product;
        }
    }
}
