namespace colanta_backend.App.Bags
{
    using System;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/bags")]
    [ApiController]
    public class BagsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [EnableCors("Ecommerce")]
        public Bag[] GetBags([FromServices] BagsService bagsService)
        {
            return bagsService.GetAvailableBags().ToArray(); //todo: caché para no consultar tanto el ERP
        }

        [HttpGet]
        [Route("/config")]
        [EnableCors("Ecommerce")]
        public ActionResult GetConfig([FromServices] BagConfigRepository bagConfigRepository)
        {
            var config = bagConfigRepository.GetConfig();
            if(config == null){
                return NotFound();
            }
            return Ok(config);
        }

        [HttpPatch]
        [Route("/config")]
        [EnableCors("Ecommerce")]
        public ActionResult SetConfig([FromServices] BagConfigRepository bagConfigRepository, [FromBody] SetBagConfigDto dto)
        {
            try{
                bagConfigRepository.CreateOrUpdateConfig(dto.BagSkuId, dto.CapacityInGrams);
                return Ok();
            }catch(Exception e){
                return BadRequest(e);
            }
        }
    }
}