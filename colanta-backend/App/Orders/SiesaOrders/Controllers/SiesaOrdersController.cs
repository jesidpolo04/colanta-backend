using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.Orders.SiesaOrders.Controllers
{
    using System.Threading.Tasks;
    using SiesaOrders.Domain;
    using SiesaOrders.Infraestructure;
    using Orders.Infraestructure;
    using Orders.Domain;

    [Route("api")]
    [ApiController]
    public class SiesaOrdersController : ControllerBase
    {
        private SiesaOrdersRepository localRepository;
        public SiesaOrdersController(SiesaOrdersRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        [HttpPost]
        [Route("siesa-orders")]
        public async Task<SiesaOrder> saveSiesaOrder(SiesaOrderDto siesaOrderDto)
        {
            SiesaOrder siesaOrder = siesaOrderDto.getSiesaOrderFromDto();
            siesaOrder = await this.localRepository.saveSiesaOrder(siesaOrder);
            return siesaOrder;
        }

        [HttpPut]
        [Route("siesa-orders")]
        public async Task<SiesaOrder> updateSiesaOrder(SiesaOrderDto siesaOrderDto)
        {
            SiesaOrder siesaOrder = siesaOrderDto.getSiesaOrderFromDto();
            siesaOrder = await this.localRepository.updateSiesaOrder(siesaOrder);
            return siesaOrder;
        }
    }
}
