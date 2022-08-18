namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.Domain;
    using Shared.Domain;
    public class NewOrderMail : INewOrderMail
    {
        private EmailSender emailSender;
        private string subject;
        private string template;
        public NewOrderMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.template = "./App/Orders/Infraestructure/Mails/NewOrderMail.cshtml";
        }

        public void SendMailToWarehouse(string wharehouseId, string orderVtexId)
        {
            this.subject = $"Tienes un nuevo pedido: #{orderVtexId}";
            NewOrderMailModel model = new NewOrderMailModel(orderVtexId);
            this.emailSender.SendEmail(this.subject, this.template, model, "jesdady482@gmail.com");
        }
    }
}
