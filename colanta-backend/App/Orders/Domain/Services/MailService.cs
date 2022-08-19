namespace colanta_backend.App.Orders.Domain
{
    using Shared.Domain;
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

        public void SendMailToWarehouse(string wharehouseId, string vtexOrderId)
        {
            this.newOrderMail.SendMailToWarehouse(wharehouseId, vtexOrderId);
        }

        public void SendSiesaErrorMail(SiesaException exception, string vtexOrderId)
        {
            this.siesaErrorAtSendOrderMail.SendMail(exception, vtexOrderId);
        }
    }
}
