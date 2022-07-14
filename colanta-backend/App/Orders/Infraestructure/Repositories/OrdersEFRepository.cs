namespace colanta_backend.App.Orders.Infraestructure
{
    using App.Shared.Infraestructure;
    using App.Orders.Domain;
    using App.Products.Domain;
    using App.Products.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Text.Json;

    public class OrdersEFRepository : Domain.OrdersRepository
    {
        private ColantaContext dbContext;

        public OrdersEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<bool> deleteOrder(Order order)
        {
            EFOrder efOrder = dbContext.Orders.Find(order.id);
            dbContext.Orders.Remove(efOrder);
            dbContext.SaveChanges();
            return true;
        }

        public async Task<Order> getOrderByVtexId(string vtexId)
        {
            EFOrder[] efOrders = this.dbContext.Orders.Where(order => order.vtex_id == vtexId).ToArray();
            if(efOrders.Length > 0)
            {
                return efOrders.First().getOrderFromEfOrder();
            }
            return null;
        }

        public async Task<Order> SaveOrder(Order order)
        {
            EFOrder efOrder = new EFOrder();
            efOrder.setEfOrderFromOrder(order);
            this.dbContext.Add(efOrder);
            this.dbContext.SaveChanges();
            return order;
        }

        public async Task<Order> SaveOrderHistory(Order order)
        {
            EFOrderHistory efOrderHistory = new EFOrderHistory();
            efOrderHistory.vtex_id = order.vtex_id;
            efOrderHistory.json = JsonSerializer.Serialize(order);
            this.dbContext.OrderHistory.Add(efOrderHistory);
            this.dbContext.SaveChanges();
            return order;
        }
    }
}
