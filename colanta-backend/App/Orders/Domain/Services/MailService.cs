namespace colanta_backend.App.Orders.Domain
{
    public class MailService
    {
        private INewOrderMail newOrderMail;

        public MailService(
            INewOrderMail newOrderMail
            )
        {
            this.newOrderMail = newOrderMail;
        }

        public void SendMailToWarehouse(string wharehouseId, string vtexOrderId)
        {
            this.newOrderMail.SendMailToWarehouse(wharehouseId, vtexOrderId);
        }
    }
}
