using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Orders.Controllers
{
    using Orders.Domain;
    using Orders.Application;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using System.Text.Json;

    [ApiController]
    [Route("api")]
    public class OrdersHookController : ControllerBase
    {
        private OrdersRepository localRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        public OrdersHookController(
            OrdersRepository localRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        [Route("orders/hook")]
        [HttpPost]
        public async Task<object> orderHook(object request)
        {
            string stringRequest = JsonSerializer.Serialize(request);
            JObject jrequest = JObject.Parse(stringRequest);
            if(jrequest.Property("hookConfig") != null) 
            {
                return new { hookConfig = "alive!" };
            }
            OrderHookDto orderSummary = JsonSerializer.Deserialize<OrderHookDto>(stringRequest);
            SaveVtexOrderInSiesa saveVtexOrderInSiesa = new SaveVtexOrderInSiesa(this.localRepository, this.vtexRepository, this.siesaRepository);
            await saveVtexOrderInSiesa.Invoke(orderSummary.getOrderFromDto());
            return orderSummary;
        }
    }
}
