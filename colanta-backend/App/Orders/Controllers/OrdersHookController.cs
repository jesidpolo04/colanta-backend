using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Orders.Controllers
{
    using System;
    using Products.Domain;
    using Orders.Domain;
    using Users.Domain;
    using Orders.Application;
    using SiesaOrders.Domain;
    using Shared.Domain;
    using Taxes.Services;
    using System.Threading.Tasks;
    using MicrosoftLogging = Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using System.Collections.Generic;

    [ApiController]
    [Route("api")]
    public class OrdersHookController : ControllerBase
    {
        private RegisterUserService registerUserService;
        private OrdersRepository localRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        /* private CanceledOrdersRepository canceledOrdersRepository; */
        private OrdersVtexRepository vtexRepository;
        private OrdersSiesaRepository siesaRepository;
        private SkusRepository skusRepository;
        private Shared.Domain.ILogger logger;
        private MicrosoftLogging.ILogger fileLogger;
        private MailService mailService;
        private EmailSender emailSender;

        public OrdersHookController(
            OrdersRepository localRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository,
            OrdersVtexRepository vtexRepository,
            OrdersSiesaRepository siesaRepository,
            /* CanceledOrdersRepository canceledOrdersRepository, */
            SkusRepository skusRepository,
            Shared.Domain.ILogger logger,
            MicrosoftLogging.ILogger fileLogger,
            MailService mailService,
            RegisterUserService registerUserService,
            EmailSender emailSender
        )
        {
            this.localRepository = localRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
            /* this.canceledOrdersRepository = canceledOrdersRepository; */
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.skusRepository = skusRepository;
            this.logger = logger;
            this.fileLogger = fileLogger;
            this.mailService = mailService;
            this.registerUserService = registerUserService;
            this.emailSender = emailSender;
        }

        [Route("orders/hook")]
        [HttpPost]
        public async Task<dynamic> orderHook(OrderHookDto request)
        {
            fileLogger.LogInformation("Order Hook Request: {Request}", JsonSerializer.Serialize(request));
            if (request.hookConfig != null)
            {
                return new { hookConfig = "alive!" };
            }
            //OrderHookDto orderSummary = JsonSerializer.Deserialize<OrderHookDto>(JsonSerializer.Serialize(request));
            OrderHookDto orderSummary = request;
/*             List<CanceledOrder> canceledOrders = await canceledOrdersRepository.GetAll();
            if(canceledOrders.Exists( canceledOrder => canceledOrder.VtexOrderId == request.OrderId )){
                return Ok();
            } */
            ProcessOrderUseCase useCase = new ProcessOrderUseCase(
                this.localRepository,
                this.siesaOrdersLocalRepository,
                this.vtexRepository,
                this.siesaRepository,
                this.skusRepository,
                this.logger,
                this.mailService,
                this.registerUserService);
            try{
                await useCase.Invoke(
                    orderSummary.OrderId,
                    orderSummary.State,
                    orderSummary.LastState,
                    orderSummary.LastChange,
                    orderSummary.CurrentChange);
                return Ok();
            }catch(Exception e){
                logger.writelog(e);
                return StatusCode(500);
            }
        }

        [Route("orders/send/{vtexOrderId}")]
        [HttpGet]
        public async Task Send(string vtexOrderId)
        {
            try
            {
                var vtexOrder = await this.vtexRepository.getOrderByVtexId(vtexOrderId);
                var useCase = new SendOrderManuallyUseCase(
                   this.localRepository,
                   this.siesaOrdersLocalRepository,
                   this.vtexRepository,
                   this.siesaRepository,
                   this.skusRepository,
                   this.mailService,
                   this.registerUserService,
                   this.emailSender);

                await useCase.Invoke(vtexOrder.orderId);
                Ok();
            }
            catch (Exception error)
            {
                this.logger.writelog(error);
                StatusCode(500, error);
            }
        }

        [Route("orders/email/{vtexOrderId}")]
        [HttpPost]
        public async Task sendNotifyEmail(string vtexOrderId)
        {
            try{
                var siesaOrder = await this.siesaOrdersLocalRepository.getSiesaOrderByVtexId(vtexOrderId);
                var vtexOrder = await this.vtexRepository.getOrderByVtexId(vtexOrderId);
                this.mailService.SendMailToWarehouse(siesaOrder.co, siesaOrder, vtexOrder);
                Ok("Sending mail.");
            }catch(Exception error){
                this.logger.writelog(error);
                StatusCode(500, error);
            }
        }
    }
}
