using colanta_backend.App.PriceTables;
using colanta_backend.App.Promotions.Domain;
using colanta_backend.App.Promotions.Jobs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace colanta_backend.App.Promotions.Presentation
{
    [Route("api/promotions/")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        PromotionsRepository _Repository;
        public PromotionsController(PromotionsRepository repository)
        {
            _Repository = repository;
        }

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

        [HttpPost]
        [Route("recalculate-promotional-prices")]
        public ActionResult RecalculatePromotionPrices([FromServices] PromotionalPricesRenderer promotionalPricesRenderer)
        {
            try
            {
                var promotions = _Repository.getVtexPromotions().Result;
                Task.Run(() =>
                {
                    foreach (var promotion in promotions)
                    {
                        if (promotion.price_table_name != null && promotion.price_table_name != "")
                        {
                            promotionalPricesRenderer.Render(promotion);
                        }
                    }
                });
                return Ok("Recalculando tablas de precios promocionales");
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
