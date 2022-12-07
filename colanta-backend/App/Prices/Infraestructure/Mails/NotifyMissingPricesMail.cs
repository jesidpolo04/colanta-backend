namespace colanta_backend.App.Prices.Infraestructure
{
    using Shared.Domain;
    using colanta_backend.App.Products.Domain;
    using Prices.Domain;
    public class NotifyMissingPricesMail : INotifyMissingPriceMail
    {
        private EmailSender emailSender;
        private string subject;

        public NotifyMissingPricesMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
        }
        public void sendMail(Sku sku)
        {
            this.subject = $"Desactivar producto {sku.siesa_id}";
            var model = new NotifyMissingPricesMailModel(sku);
            this.emailSender.SendEmail(this.subject, "./App/Prices/Infraestructure/Mails/NotifyMissingPricesMail.cshtml", model, "jesdady482@gmail.com");
        }
    }
}
