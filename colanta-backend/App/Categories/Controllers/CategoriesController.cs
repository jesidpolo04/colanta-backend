using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Categories.Controllers
{
    using App.Categories.Jobs;
    using colanta_backend.App.Auth.Middlewares;

    [Route("api/categories")]
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]

    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Render([FromServices] RenderCategories job)
        {
            job.Invoke();
            Ok("Rendering categories");
        }
    }
}