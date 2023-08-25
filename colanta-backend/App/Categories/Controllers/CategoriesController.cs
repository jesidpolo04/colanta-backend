using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Categories.Controllers
{
    using App.Categories.Jobs;
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Render([FromServices] RenderCategories job){
            job.Invoke();
            Ok("Rendering categories");
        }
    }
}