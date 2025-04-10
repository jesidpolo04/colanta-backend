using System;
using System.Threading;
using System.Threading.Tasks;
using colanta_backend.App.PriceTables.Scripts;
using colanta_backend.App.Products.Domain;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.PriceTables
{
    [Route("api/price-tables/")]
    [ApiController]
    public class FixedPriceController : ControllerBase
    {
        PriceTablesVtexService _Service;
        PriceTablesRepository _Repository;
        public FixedPriceController(PriceTablesVtexService service, PriceTablesRepository repository)
        {
            _Service = service;
            _Repository = repository;
        }

        [HttpPost]
        [Route("render-price-table/{promotionSiesaId}")]
        public ActionResult RenderPriceTable([FromServices] RenderPriceTable script, string promotionSiesaId)
        {
            Task.Run(() =>
            {
                script.Execute(promotionSiesaId).Wait();
            });
            return Ok("Renderizando precios fijos en VTEX.");
        }

        [HttpPost]
        [Route("recalculate-cluster-fixed-prices")]
        public ActionResult RenderClusterFixedPrices([FromServices] RecalculateFixedPrices script){
            Task.Run(()=>{
                script.Execute();
            });
            return Ok("Recalculando precios fijos en VTEX.");
        }

                [HttpPost]
        [Route("recalculate-non-cluster-fixed-prices")]
        public ActionResult RenderNonClusterFixedPrices([FromServices] RecalculateNonClusterFixedPrices script){
            Task.Run(()=>{
                script.Execute();
            });
            return Ok("Recalculando precios fijos en VTEX.");
        }

        [HttpPost]
        [Route("render-fixed-prices-in-bd")]
        public ActionResult RenderFixedPricesInBd([FromServices] RenderFixedPricesInBd script){
            Task.Run(()=>{
                script.Execute();
            });
            return Ok("Renderizando precios fijos en base de datos.");
        }

        [HttpPost]
        [Route("delete-all-fixed-prices")]
        public ActionResult DeleteAllFixedPrices([FromServices] SkusRepository skusRepository)
        {
            try
            {
                var skus = skusRepository.getVtexSkus().Result;
                var priceTables = _Repository.GetAll();
                Task.Run(() =>
                {
                    foreach (var priceTable in priceTables)
                    {
                        Console.WriteLine($"Limpiando tabla de precios: {priceTable.Name}");
                        foreach (var sku in skus)
                        {
                            _Service.DeleteFixedPriceFromPriceTable(priceTable.Name, (int)sku.vtex_id);
                            Thread.Sleep(90);
                        }
                    }
                });
                return Ok("Borrando todos los precios fijos");
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}