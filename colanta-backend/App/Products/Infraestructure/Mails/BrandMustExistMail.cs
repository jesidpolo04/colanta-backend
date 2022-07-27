namespace colanta_backend.App.Products.Infraestructure { 

    using Shared.Domain;
    using Products.Domain;

    public class BrandMustExistMail
    {
        private EmailSender emailSender;
        private string subject;
        private BrandMustExistException exception;
        public BrandMustExistMail(BrandMustExistException exception, EmailSender emailSender)
        {
            this.exception = exception;
            this.emailSender = emailSender;
            this.subject = $"Producto: {exception.product.name} no posee una categoría válida";
        }

        public async void sendMail()
        {
            BrandMustExistMailModel emailModel = new BrandMustExistMailModel(exception.product);
            this.emailSender.SendEmail(this.subject, "./App/Products/Domain/Mails/BrandMustExistMail.cshtml", emailModel);
        }
    }
}
