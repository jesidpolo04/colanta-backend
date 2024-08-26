namespace colanta_backend.App.Orders.Infraestructure
{
    using Shared.Infraestructure;
    using Orders.Domain;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    public class CanceledOrdersEFRepository : CanceledOrdersRepository
    {
        private ColantaContext DbContext;
        public CanceledOrdersEFRepository(IConfiguration configuration)
        {
            DbContext = new ColantaContext(configuration);
        }

        public async Task<List<CanceledOrder>> GetAll()
        {
            var canceledOrders = DbContext.CanceledOrders;
            return canceledOrders.Select(canceledOrder => canceledOrder.getCanceledOrder()).ToList();
        }
    }
}
