namespace colanta_backend.App.Orders.Domain
{
    using Shared.Domain;
    using Orders.SiesaOrders.Domain;
    public class MailService
    {
        private INewOrderMail newOrderMail;
        private ISiesaErrorAtSendOrderMail siesaErrorAtSendOrderMail;

        public MailService(
            INewOrderMail newOrderMail,
            ISiesaErrorAtSendOrderMail siesaErrorMail
            )
        {
            this.newOrderMail = newOrderMail;
            this.siesaErrorAtSendOrderMail = siesaErrorMail;
        }

        public void SendMailToWarehouse(string wharehouseId, SiesaOrder siesaOrder)
        {
            this.newOrderMail.SendMailToWarehouse(wharehouseId, siesaOrder);
        }

        public void SendSiesaErrorMail(SiesaException exception, string vtexOrderId)
        {
            this.siesaErrorAtSendOrderMail.SendMail(exception, vtexOrderId);
        }
    }
}
