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

    public class OrdersEFRepository : Domain.OrdersRepository
    {
        private ColantaContext dbContext;

        public OrdersEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<Order> SaveOrder(Order order)
        {
            EFOrder efOrder = new EFOrder();
            efOrder.setEfOrderFromOrder(order);
            this.dbContext.Add(efOrder);
            this.dbContext.SaveChanges();
            return order;
        }
    }
}
