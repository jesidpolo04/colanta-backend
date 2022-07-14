namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    public interface OrdersRepository
    {
        Task<Order> SaveOrder(Order order);
        Task<Order> getOrderByVtexId(string vtexId);
        Task<Order> SaveOrderHistory(Order order);
        Task<bool> deleteOrder(Order order);
    }
}
