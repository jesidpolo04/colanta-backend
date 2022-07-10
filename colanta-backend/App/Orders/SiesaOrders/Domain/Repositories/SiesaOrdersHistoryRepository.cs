namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    using System.Threading.Tasks;
    public interface SiesaOrdersHistoryRepository
    {
        Task<SiesaOrderHistory> saveSiesaOrderHistory(SiesaOrder siesaOrder);
    }
}
