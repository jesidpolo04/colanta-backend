using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Orders.Controllers
{
    using Orders.Domain;
    using Orders.Application;
    using System.Threading.Tasks;

    [Route("api/orders")]
    [ApiController]
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
        [Route("/hook")]
        [HttpPost]
        public async Task<OrderHookDto> orderHook(OrderHookDto orderSummary)
        {
            SaveVtexOrderInSiesa saveVtexOrderInSiesa = new SaveVtexOrderInSiesa(this.localRepository, this.vtexRepository, this.siesaRepository);
            await saveVtexOrderInSiesa.Invoke(orderSummary.getOrderFromDto());
            return orderSummary;
        }
    }
}
