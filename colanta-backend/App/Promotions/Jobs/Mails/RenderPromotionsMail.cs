namespace colanta_backend.App.Promotions.Jobs
{
    using Shared.Domain;
    using Promotions.Domain;
    using System;
    using System.Collections.Generic;
    public class RenderPromotionsMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        public string emailTitle = "Renderizado de Promociones";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;

        public RenderPromotionsMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Promotion[] inactivePromotions, Promotion[] failedLoadPromotions)
        {
            bool sendEmail = false;
            string body = "";

            if (inactivePromotions.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Promociones Inactivas en VTEX") + "\n";
                List<string> inactivePromotionsInfo = new List<string>();
                foreach (Promotion inactivePromotion in inactivePromotions)
                {
                    inactivePromotionsInfo.Add($"{inactivePromotion.name}, con SIESA id: {inactivePromotion.siesa_id} y VTEX id: {inactivePromotion.vtex_id}");
                }
                body += htmlWriter.ul(inactivePromotionsInfo.ToArray());
                body += "\n";
            }

            if (failedLoadPromotions.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Promocoens que fallaron al cargarse a VTEX") + "\n";
                List<string> failedLoadPromotionsInfo = new List<string>();
                foreach (Promotion failedLoadPromotion in failedLoadPromotions)
                {
                    failedLoadPromotionsInfo.Add($"{failedLoadPromotion.name}, con SIESA id: {failedLoadPromotion.siesa_id}");
                }
                body += htmlWriter.ul(failedLoadPromotionsInfo.ToArray());
                body += "\n";
            }

            if (sendEmail)
            {
                //this.emailSender.SendEmail(this.emailTitle, body);
            }
        }
    }
}
