using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Products.Controllers
{
    using App.Products.Jobs;
    using colanta_backend.App.Auth.Middlewares;

    [Route("api/products")]
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]

    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Render([FromServices] RenderProductsAndSkus job)
        {
            job.Invoke();
            Ok("Rendering products and skus");
        }
    }
}