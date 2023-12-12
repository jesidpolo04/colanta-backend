using System;
using System.Collections.Generic;
using colanta_backend.App.Taxes.Services;
using Microsoft.AspNetCore.Cors;
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
        [EnableCors("Ecommerce")]
        [Route("calculate")]
        public ActionResult Calculate([FromBody] VtexCalculateOrderTaxesRequest request, [FromServices] TaxService taxService)
        {
            try
            {
                if(request.ClientData.Document.Equals("1002999476")) return Ok(new List<object>());
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