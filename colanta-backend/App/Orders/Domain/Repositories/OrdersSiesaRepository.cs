namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    public interface OrdersSiesaRepository
    {
        Task<Order> saveOrder(Order order);
    }
}
