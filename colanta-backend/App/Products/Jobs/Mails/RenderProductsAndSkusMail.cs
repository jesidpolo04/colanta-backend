namespace colanta_backend.App.Products.Jobs
{
    using Shared.Domain;
    using Products.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderProductsAndSkusMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        public string emailTitle = "Renderizado de Productos";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;
        public RenderProductsAndSkusMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Sku[] inactiveSkus, Sku[] failedSkus, Sku[] loadSkus)
        {
            bool sendEmail = false;
            string body = "";

            if (loadSkus.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Productos cargados en VTEX") + "\n";
                List<string> loadSkusInfo = new List<string>();
                foreach (Sku loadSku in loadSkus)
                {
                    loadSkusInfo.Add($"{loadSku.name}, con SIESA id: {loadSku.siesa_id} y VTEX id: {loadSku.vtex_id}");
                }
                body += htmlWriter.ul(loadSkusInfo.ToArray());
                body += "\n";
            }

            if (inactiveSkus.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Productos Inactivos en VTEX") + "\n";
                List<string> inactiveSkusInfo = new List<string>();
                foreach (Sku inactiveSku in inactiveSkus)
                {
                    inactiveSkusInfo.Add($"{inactiveSku.name}, con SIESA id: {inactiveSku.siesa_id} y VTEX id: {inactiveSku.vtex_id}");
                }
                body += htmlWriter.ul(inactiveSkusInfo.ToArray());
                body += "\n";
            }

            if (failedSkus.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Productos que fallaron al cargarse a VTEX") + "\n";
                List<string> failedSkusInfo = new List<string>();
                foreach (Sku failedSku in failedSkus)
                {
                    failedSkusInfo.Add($"{failedSku.name}, con SIESA id: {failedSku.siesa_id}");
                }
                body += htmlWriter.ul(failedSkusInfo.ToArray());
                body += "\n";
            }

            if (sendEmail)
            {
                this.emailSender.SendEmail(this.emailTitle, body);
            }
        }
    }
}
