using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace colanta_backend.App.Taxes.Controllers
{
    [Route("api/taxes")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        [HttpPost]
        [Route("calculate")]
        public ActionResult Render([FromBody] object order)
        {
            Console.WriteLine("Entró al método");
            var json = JsonConvert.SerializeObject(order, Formatting.Indented);
            Console.WriteLine(json);
            /* HttpContext.Response.Headers.Add("ngrok-skip-browser-warning", "123"); */
            var arreglo = new object[]{};
            return Ok(arreglo);
        }
    }
}