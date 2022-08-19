namespace colanta_backend.App.Orders.Domain
{
    using Orders.SiesaOrders.Domain;
    public interface INewOrderMail
    {
        void SendMailToWarehouse(string wharehouseId, SiesaOrder siesaOrder);
    }
}
