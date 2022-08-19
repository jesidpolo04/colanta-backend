namespace colanta_backend.App.Orders.Infraestructure
{
    using Shared.Domain;
    using Orders.Domain;
    public class SiesaErrorAtSendOrderMail : ISiesaErrorAtSendOrderMail
    {
        private EmailSender emailSender;
        private string subject;
        private string template;

        public SiesaErrorAtSendOrderMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.template = "./App/Orders/Infraestructure/Mails/SiesaErrorAtSendOrderMail.cshtml";
        }
        
        public void SendMail(SiesaException siesaException, string vtexOrderId)
        {
            this.subject = $"Error al registrar el pedido #{vtexOrderId}";
            SiesaErrorAtSendOrderMailModel model = new SiesaErrorAtSendOrderMailModel(siesaException, vtexOrderId);
            this.emailSender.SendEmail(this.subject, this.template, model, EmailAddresses.Tech);
        }
    }
}
