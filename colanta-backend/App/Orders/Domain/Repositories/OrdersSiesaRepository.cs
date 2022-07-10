namespace colanta_backend.App.Orders.Domain
{
    using System.Threading.Tasks;
    using Orders.SiesaOrders.Domain;
    public interface OrdersSiesaRepository
    {
        Task<SiesaOrder> saveOrder(Order order);
        Task<SiesaOrder> getOrderBySiesaId(string siesaId);
    }
}
