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
    using System.Dynamic;

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
        public async Task<dynamic> orderHook(OrderHookDto request)
        {
            if(request.hookConfig != null) 
            {
                return new { hookConfig = "alive!" };
            }
            //OrderHookDto orderSummary = JsonSerializer.Deserialize<OrderHookDto>(JsonSerializer.Serialize(request));
            OrderHookDto orderSummary = request;
            ProcessOrderUseCase useCase = new ProcessOrderUseCase(
                this.localRepository, 
                this.siesaOrdersLocalRepository, 
                this.vtexRepository, 
                this.siesaRepository);

            await useCase.Invoke(
                orderSummary.OrderId, 
                orderSummary.State, 
                orderSummary.LastState, 
                orderSummary.LastChange,
                orderSummary.CurrentChange);
            return Ok();
        }
    }
}
