namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    public interface OrdersRepository
    {
        Task<Order> SaveOrder(Order order);
    }
}
