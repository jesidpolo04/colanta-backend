using colanta_backend.App.Promotions.Jobs;
using Microsoft.AspNetCore.Mvc;
using System;

namespace colanta_backend.App.Promotions.Presentation
{
    [Route("api/promotions/")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        [HttpPost]
        [Route("render")]
        public void Post([FromServices] RenderPromotions job)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authorization);
                if (authorization.Equals("Jesing0408"))
                {
                    job.Invoke().Wait();
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
