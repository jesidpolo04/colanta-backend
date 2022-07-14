namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    using SiesaOrders.Domain;
    public interface OrdersVtexRepository
    {
        Task<string> getOrderByVtexId(string vtexOrderId);
        Task<string> updateVtexOrder(SiesaOrder oldOrder, SiesaOrder newOrder);
        Task<bool> startHandlingOrder(string orderVtexId);
        Task<OrderStatus> getOrderStatus(string orderVtexId);
        Task<PaymentMethod> getOrderPaymentMethod(string orderVtexId);
    }
}
