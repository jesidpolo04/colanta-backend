using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string HealthCheck()
        {
            return "middleware Colanta, ¡is ready for use!";
        }
    }
}
