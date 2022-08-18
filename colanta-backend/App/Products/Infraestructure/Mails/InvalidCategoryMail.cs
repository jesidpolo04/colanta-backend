namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    using Shared.Domain;
    public class InvalidCategoryMail : IInvalidCategoryMail
    {
        private EmailSender emailSender;
        private string subject;
        
        public InvalidCategoryMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "Producto no posee una categoría válida";
        }

        public void sendMail(InvalidCategoryException exception)
        {
            InvalidCategoryMailModel model = new InvalidCategoryMailModel(exception.product);
            this.emailSender.SendEmail(this.subject, "./App/Products/Infraestructure/Mails/InvalidCategoryMail.cshtml", model, EmailAddresses.Tech);
        }
    }
}
