namespace colanta_backend.App.OrderObservations.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using colanta_backend.App.OrderObservations.Domain;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    
    [Route("api/product_observations")]
    [ApiController]
    public class OrderObservationsController : ControllerBase
    {
        private readonly OrderObservationsRepository _repository;
        private readonly ILogger<OrderObservationsController> _logger;

        public OrderObservationsController(
            OrderObservationsRepository repository,
            ILogger<OrderObservationsController> logger
        )
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/OrderObservations
        [HttpGet]
        [Route("fields")]
        [EnableCors("EcommerceWildCard")]
        public async Task<IActionResult> GetProductObservationFields()
        {
            try
            {
                return Ok((await _repository.GetOrderObservationFields()).Select(field => new ObservationFieldDto
                {
                    Id = field.Id,
                    Code = field.Code,
                    Description = field.Description
                }));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting product observation fields");
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("cut_types")]
        [EnableCors("EcommerceWildCard")]
        public async Task<IActionResult> GetProductCutTypesValues()
        {
            try
            {
                return Ok((await _repository.GetProductCutTypeValues()).Select(cutType => new CutTypeDto
                {
                    Id = cutType.Id,
                    Code = cutType.Code,
                    Description = cutType.Description
                }));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting product cut type values");
                return BadRequest(e.Message);
            }
        }
    }
}