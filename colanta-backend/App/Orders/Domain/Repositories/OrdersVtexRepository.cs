namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    public interface OrdersVtexRepository
    {
        Task<string> getOrderByVtexId(string vtexOrderId);
    }
}
