namespace colanta_backend.App.Orders.Domain
{
    using Shared.Domain;
    public interface ISiesaErrorAtSendOrderMail
    {
        public void SendMail(SiesaException siesaException, string vtexOrderId);
    }
}
