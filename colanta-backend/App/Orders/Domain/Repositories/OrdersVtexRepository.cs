namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    using SiesaOrders.Domain;
    public interface OrdersVtexRepository
    {
        Task<VtexOrder> getOrderByVtexId(string vtexOrderId);
        Task<string> updateVtexOrder(SiesaOrder oldOrder, SiesaOrder newOrder);
        Task<bool> startHandlingOrder(string orderVtexId);
    }
}
