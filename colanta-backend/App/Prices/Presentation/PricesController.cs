using colanta_backend.App.Auth.Middlewares;
using colanta_backend.App.Prices.Jobs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace colanta_backend.App.Prices.Presentation
{
    [Route("api/prices/")]
    [ApiController]
    [ServiceFilter(typeof(AuthFilter))]

    public class PricesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Post([FromServices] RenderPrices job)
        {
            try
            {
                job.Invoke().Wait();
                Ok("Renderizando precios");
            }
            catch (Exception exception)
            {
                StatusCode(500, exception.Message);
            }
        }
    }
}
