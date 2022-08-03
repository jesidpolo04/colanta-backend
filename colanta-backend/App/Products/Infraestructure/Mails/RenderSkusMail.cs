namespace colanta_backend.App.Products.Infraestructure
{
    using System.Collections.Generic;
    using Shared.Domain;
    using Products.Domain;
    public class RenderProductsMail : IRenderProductsMail
    {
        private EmailSender emailSender;
        private string subject;

        public RenderProductsMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "Renderizado de productos";
        }

        public void sendMail(List<Sku> loadedSkus, List<Sku> inactivatedSkus, List<Sku> failedSkus)
        {
            bool sendMail = false;
            sendMail = loadedSkus.Count > 0 ? true : sendMail;
            sendMail = inactivatedSkus.Count > 0 ? true : sendMail;
            sendMail = failedSkus.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderProductsMailModel model = new RenderProductsMailModel(loadedSkus, inactivatedSkus, failedSkus);
                this.emailSender.SendEmail(subject, "./App/Products/Infraestructure/Mails/RenderProductsMail.cshtml", model);
            }
        }
    }
}
