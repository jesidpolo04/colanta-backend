

namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System;
    using Microsoft.Extensions.Configuration;
    using Shared.Infraestructure;
    public class SiesaOrdersHistoryEFRepository : Domain.SiesaOrdersHistoryRepository
    {
        private ColantaContext dbContext;

        public SiesaOrdersHistoryEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<SiesaOrderHistory> saveSiesaOrderHistory(SiesaOrder siesaOrder)
        {
            string vtexId = siesaOrder.referencia_vtex;
            string siesaId = siesaOrder.siesa_id;
            string siesaOrderJson = JsonSerializer.Serialize(siesaOrder);
            EFSiesaOrderHistory siesaOrderHistory = new EFSiesaOrderHistory();
            siesaOrderHistory.siesa_id = siesaId;
            siesaOrderHistory.vtex_id = vtexId;
            siesaOrderHistory.order_json = siesaOrderJson;
            siesaOrderHistory.created_at = DateTime.Now;

            dbContext.Add(siesaOrderHistory);
            dbContext.SaveChanges();
            return siesaOrderHistory.getSiesaOrderHistoryFromEfSiesaOrderHistory();
        }
    }
}
