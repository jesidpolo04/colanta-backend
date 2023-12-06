using System;
using colanta_backend.App.Taxes.Services;
using Microsoft.AspNetCore.Mvc;
namespace colanta_backend.App.Taxes.Controllers
{
    [Route("api/taxes")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        [HttpPost]
        [Route("calculate")]
        public ActionResult Calculate([FromBody] VtexCalculateOrderTaxesRequest request, [FromServices] TaxService taxService)
        {
            try
            {
                var useCase = new CalculateOrderTaxes(taxService);
                var response = useCase.Execute(request);
                return Ok(response);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return BadRequest();
            }
        }
    }
}