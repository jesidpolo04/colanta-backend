namespace colanta_backend.App.Orders.Domain
{
    public interface INewOrderMail
    {
        void SendMailToWarehouse(string wharehouseId, string orderVtexId);
    }
}
