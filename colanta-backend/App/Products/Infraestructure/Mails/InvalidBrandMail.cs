namespace colanta_backend.App.Products.Infraestructure { 

    using Shared.Domain;
    using Products.Domain;

    public class InvalidBrandMail : IInvalidBrandMail
    {
        private EmailSender emailSender;
        private string subject;
        public InvalidBrandMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = $"Producto no posee una marca válida";
        }

        public async void sendMail(InvalidBrandException exception)
        {
            InvalidBrandMailModel emailModel = new InvalidBrandMailModel(exception.product);
            this.emailSender.SendEmail(this.subject, "./App/Products/Infraestructure/Mails/InvalidBrandMail.cshtml", emailModel);
        }
    }
}
