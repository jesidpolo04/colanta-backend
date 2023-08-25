using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Products.Controllers
{
    using App.Products.Jobs;
    [Route("api/products")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Render([FromServices] RenderProductsAndSkus job){
            job.Invoke();
            Ok("Rendering products and skus");
        }
    }
}