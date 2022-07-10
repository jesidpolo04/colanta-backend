namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    using System.Threading.Tasks;
    public interface SiesaOrdersRepository
    {
        Task<SiesaOrder> saveSiesaOrder(SiesaOrder siesaOrder);
        Task<SiesaOrder> getSiesaOrderBySiesaId(string siesaId);
        Task<SiesaOrder> getSiesaOrderByVtexId(string vtexId);
        Task<SiesaOrder[]> getAllSiesaOrdersByFinalizado(bool finalizado);

        Task<SiesaOrder> updateSiesaOrder(SiesaOrder siesaOrder);

        Task<SiesaOrderDetail> updateSiesaOrderDetail(SiesaOrderDetail siesaOrderDetail);
        Task<SiesaOrderDetail> deleteSiesaOrderDetail(SiesaOrderDetail siesaOrderDetail);

        Task<SiesaOrderDiscount> updateSiesaOrderDiscount(SiesaOrderDiscount siesaOrderDiscount);
        Task<SiesaOrderDiscount> deleteSiesaOrderDetail(SiesaOrderDiscount siesaOrderDiscounts);

    }
}
