using System;
using colanta_backend.App.Taxes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace colanta_backend.App.Taxes.Controllers
{
    [Route("api/taxes")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        private ILogger<TaxesController> _Logger;

        public TaxesController(ILogger<TaxesController> Logger)
        {
            _Logger = Logger;
        }

        [HttpPost]
        [Route("calculate")]
        public ActionResult Calculate([FromBody] VtexCalculateOrderTaxesRequest request, [FromServices] TaxService taxService)
        {
            try
            {
                var useCase = new CalculateOrderTaxes(taxService, _Logger);
                var response = useCase.Execute(request);
                return Ok(response);
            }
            catch (Exception error)
            {
                _Logger.LogError(error.ToString());
                return BadRequest();
            }
        }
    }
}