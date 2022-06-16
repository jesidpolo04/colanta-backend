using colanta_backend.App.Orders.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Orders.Infraestructure
{
    public class OrdersSiesaRepository : Domain.OrdersSiesaRepository
    {
        public async Task<Order> saveOrder(Order order)
        {
            return order;
        }
    }
}
