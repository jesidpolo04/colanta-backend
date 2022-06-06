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
        private string[] headers = { "siesa_id", "vtex_id", "nombre_producto", "antiguo_precio", "nuevo_precio" };
        public string emailTitle = "Renderizado de Precios";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;

        public RenderPricesMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Price[] loadPrices, Price[] updatedPrices, Price[] failedPrices)
        {
            string mailBody = "<html>";
            mailBody += "<head>";
            mailBody += "<style>";
            mailBody += "td, th{border: 1px solid black; padding: 2px 3px}";
            mailBody += "</style>";
            mailBody += "</head>";
            mailBody += "<body>";
            mailBody += htmlWriter.h("1", this.emailTitle);
            mailBody += htmlWriter.h("2", this.emailSubtitle);

            if (loadPrices.Length > 0)
            {
                List<string[]> stringPrices = new List<string[]>();
                foreach (Price price in loadPrices)
                {
                    string[] stringPrice = { price.sku.siesa_id, price.sku.vtex_id.ToString(), price.sku.name, " - ", price.price.ToString() };
                    stringPrices.Add(stringPrice);
                }
                mailBody += htmlWriter.h("3", "Precios cargados a Vtex");
                mailBody += htmlWriter.table(this.headers, stringPrices.ToArray());
            }

            if (failedPrices.Length > 0)
            {
                List<string[]> stringPrices = new List<string[]>();
                foreach (Price price in failedPrices)
                {
                    string[] stringPrice = { price.sku.siesa_id, price.sku.vtex_id.ToString(), price.sku.name, " - ", price.price.ToString() };
                    stringPrices.Add(stringPrice);
                }
                mailBody += htmlWriter.h("3", "Precios que no fueron cargados a Vtex debido a error");
                mailBody += htmlWriter.table(this.headers, stringPrices.ToArray());
            }

            if (updatedPrices.Length > 0)
            {
                List<string[]> stringPrices = new List<string[]>();
                foreach (Price price in updatedPrices)
                {
                    string[] stringPrice = { price.sku.siesa_id, price.sku.vtex_id.ToString(), price.sku.name, " - ", price.price.ToString() };
                    stringPrices.Add(stringPrice);
                }
                mailBody += htmlWriter.h("3", "Precios actualizados en Vtex");
                mailBody += htmlWriter.table(this.headers, stringPrices.ToArray());
            }

            mailBody += "</body> </html>";
            this.emailSender.SendEmail(this.emailTitle + " " + this.dateTime.ToString(), mailBody);
        }
    }
}
