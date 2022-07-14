namespace colanta_backend.App.Orders.Domain.Repositories
{
    using System.Threading.Tasks;
    public interface OrderStatusRepository
    {
        Task<OrderStatus[]> getAllOrderStatus();
    }
}
