namespace colanta_backend.App.Prices.Infraestructure
{
    using Prices.Domain;
    using Shared.Domain;
    using System.Collections.Generic;
    public class RenderPricesMail : IRenderPricesMail
    {
        private EmailSender emailSender;
        private string subject;

        public RenderPricesMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "Renderizado de precios";
        }

        public void sendMail(List<Price> loadedPrices, List<Price> updatedPrices, List<Price> failedPrices)
        {
            bool sendMail = false;
            sendMail = loadedPrices.Count > 0 ? true : sendMail;
            sendMail = updatedPrices.Count > 0 ? true : sendMail;
            sendMail = failedPrices.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderPricesMailModel model = new RenderPricesMailModel(loadedPrices, updatedPrices, failedPrices);
                this.emailSender.SendEmail(this.subject, "./App/Prices/Infraestructure/Mails/RenderPricesMail.cshtml", model);
            }
        }
    }
}
