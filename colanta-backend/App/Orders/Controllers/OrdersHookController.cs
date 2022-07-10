using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Orders.Controllers
{
    using Orders.Domain;
    using Orders.Application;
    using SiesaOrders.Domain;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;
    using System.Text.Json;

    [ApiController]
    [Route("api")]
    public class OrdersHookController : ControllerBase
    {
        private OrdersRepository localRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        public OrdersHookController(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
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
            SaveVtexOrderInSiesa saveVtexOrderInSiesa = new SaveVtexOrderInSiesa(this.localRepository, this.siesaOrdersLocalRepository, this.vtexRepository, this.siesaRepository);
            SiesaOrder siesaOrder = await saveVtexOrderInSiesa.Invoke(orderSummary.getOrderFromDto());
            return siesaOrder;
        }
    }
}
