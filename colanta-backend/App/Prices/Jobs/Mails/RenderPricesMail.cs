namespace colanta_backend.App.Prices.Jobs
{
    using Shared.Domain;
    using Prices.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderPricesMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        public string emailTitle = "Renderizado de Precios";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;

        public RenderPricesMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Price[] failedPrices, Price[] loadPrices, Price[] updatePrices)
        {
            bool sendEmail = false;
            string body = "";

            if (failedPrices.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Precios que no pudieron ser asignados en VTEX") + "\n";
                List<string> failedPricesInfo = new List<string>();
                foreach (Price failedPrice in failedPrices)
                {
                    failedPricesInfo.Add($"{failedPrice.sku.name} ({failedPrice.sku.siesa_id}), precio: {string.Format("{0:C}", failedPrice.price)}");
                }
                body += htmlWriter.ul(failedPricesInfo.ToArray());
                body += "\n";
            }

            if (loadPrices.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Precios asignados en VTEX") + "\n";
                List<string> loadPricesInfo = new List<string>();
                foreach (Price loadPrice in loadPrices)
                {
                    loadPricesInfo.Add($"{loadPrice.sku.name} ({loadPrice.sku.siesa_id}), precio: {string.Format("{0:C}", loadPrice.price)}");
                }
                body += htmlWriter.ul(loadPricesInfo.ToArray());
                body += "\n";
            }

            if (updatePrices.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Precios actualizados en VTEX") + "\n";
                List<string> updatePricesInfo = new List<string>();
                foreach (Price updatePrice in updatePrices)
                {
                    updatePricesInfo.Add($"{updatePrice.sku.name} ({updatePrice.sku.siesa_id}), precio: {string.Format("{0:C}", updatePrice.price)}");
                }
                body += htmlWriter.ul(updatePricesInfo.ToArray());
                body += "\n";
            }

            if (sendEmail)
            {
                //this.emailSender.SendEmail(this.emailTitle, body);
            }
        }
    }
}
