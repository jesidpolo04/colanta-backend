using colanta_backend.App.Prices.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace colanta_backend.App.Prices.Presentation
{
    [Route("api/prices/")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Post([FromServices] RenderPrices job)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authorization);
                if (authorization.Equals("Jesing0408"))
                {
                    job.Invoke();
                    Ok("Renderizando precios");
                }
                else
                {
                    Unauthorized();
                }
            }
            catch (Exception exception)
            {
                StatusCode(500, exception.Message);
            }
        }
    }
}
