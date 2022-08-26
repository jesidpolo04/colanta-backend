namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    using SiesaOrders.Domain;
    public interface OrdersVtexRepository
    {
        Task<VtexOrder> getOrderByVtexId(string vtexOrderId);
        Task<string> updateVtexOrder(SiesaOrder oldOrder, SiesaOrder newOrder);
        Task startHandlingOrder(string orderVtexId);
        Task approvePayment(string paymentId, string orderVtexId);
        Task cancelOrder(string orderVtexId);
    }
}
