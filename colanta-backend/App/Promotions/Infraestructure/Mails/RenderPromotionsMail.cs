namespace colanta_backend.App.Promotions.Infraestructure
{
    using System.Collections.Generic;
    using Promotions.Domain;
    using Shared.Domain;
    public class RenderPromotionsMail : IRenderPromotionsMail
    {
        private EmailSender emailSender;
        private string subject = "Renderizado de promociones";
        private string template = "./App/Promotions/Infraestructure/Mails/RenderPromotionsMail.cshtml";

        public RenderPromotionsMail(EmailSender emailSender)
        {
            this.emailSender = emailSender; 
        }

        public void sendMail(List<Promotion> loadedPromotions, List<Promotion> inactivatedPromotions, List<Promotion> failedPromotions)
        {
            bool sendMail = false;
            sendMail = loadedPromotions.Count > 0 ? true : sendMail;
            sendMail = inactivatedPromotions.Count > 0 ? true : sendMail;
            sendMail = failedPromotions.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderPromotionsMailModel model = new RenderPromotionsMailModel(loadedPromotions, inactivatedPromotions, failedPromotions);
                this.emailSender.SendEmail(this.subject, this.template, model);
            }

        }
    }
}
