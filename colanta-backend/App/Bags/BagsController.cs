namespace colanta_backend.App.Bags {
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/bags")]
    [ApiController]
    public class BagsController : ControllerBase {
        [HttpGet]
        [Route("")]
        [EnableCors("Ecommerce")]
        public Bag[] GetBags([FromServices] BagsService bagsService){
            return bagsService.GetAvailableBags().ToArray(); //todo: caché para no consultar tanto el ERP
        }
    }
}