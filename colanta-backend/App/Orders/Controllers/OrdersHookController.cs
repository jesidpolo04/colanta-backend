using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Orders.Controllers
{
    using Products.Domain;
    using Orders.Domain;
    using Users.Domain;
    using Orders.Application;
    using SiesaOrders.Domain;
    using Shared.Domain;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api")]
    public class OrdersHookController : ControllerBase
    {
        private RegisterUserService registerUserService;
        private OrdersRepository localRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        private SkusRepository skusRepository;
        private ILogger logger;
        private MailService mailService;
        public OrdersHookController(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository,
            SkusRepository skuRepository,
            ILogger logger,
            MailService mailService,
            RegisterUserService registerUserService
            )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.logger = logger;
            this.mailService = mailService;
            this.registerUserService = registerUserService;
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
                this.siesaRepository,
                this.skusRepository,
                this.logger,
                this.mailService,
                this.registerUserService);

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
