using colanta_backend.App.Auth.Middlewares;
using colanta_backend.App.Promotions.Jobs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace colanta_backend.App.Promotions.Presentation
{
    [Route("api/promotions/")]
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]

    public class PricesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Post([FromServices] RenderPromotions job)
        {
            try
            {
                job.Invoke().Wait();
                Ok("Renderizando promociones");
            }
            catch (Exception exception)
            {
                StatusCode(500, exception.Message);
            }
        }
    }
}
