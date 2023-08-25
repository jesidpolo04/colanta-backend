using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Brands.Controllers
{
    using App.Brands.Jobs;
    using Auth.Middlewares;

    [Route("api/brands")]
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]
    public class BrandsController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Render([FromServices] RenderBrands job){
            job.Invoke();
            Ok("Rendering brands");
        }
    }
}