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
        private string[] headers = { "siesa_id", "vtex_id", "nombre" };
        public string emailTitle = "Renderizado de Productos";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;
        public RenderProductsAndSkusMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Sku[] loadSkus, Sku[] inactiveSkus, Sku[] failedSkus, Sku[] notProccecedSkus, Sku[] inactivatedSkus)
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

            if (loadSkus.Length > 0)
            {
                List<string[]> stringSkus = new List<string[]>();
                foreach (Sku sku in loadSkus)
                {
                    string[] stringSku = { sku.siesa_id, sku.vtex_id.ToString(), sku.name };
                    stringSkus.Add(stringSku);
                }
                mailBody += htmlWriter.h("3", "Productos (sku) cargados a Vtex");
                mailBody += htmlWriter.table(this.headers, stringSkus.ToArray());
            }

            if (failedSkus.Length > 0)
            {
                List<string[]> stringSkus = new List<string[]>();
                foreach (Sku sku in failedSkus)
                {
                    string[] stringSku = { sku.siesa_id, sku.vtex_id.ToString(), sku.name };
                    stringSkus.Add(stringSku);
                }
                mailBody += htmlWriter.h("3", "Productos (sku) que no se cargaron en Vtex (errores)");
                mailBody += htmlWriter.table(this.headers, stringSkus.ToArray());
            }

            if (inactiveSkus.Length > 0)
            {
                List<string[]> stringSkus = new List<string[]>();
                foreach (Sku sku in inactiveSkus)
                {
                    string[] stringSku = { sku.siesa_id, sku.vtex_id.ToString(), sku.name };
                    stringSkus.Add(stringSku);
                }
                mailBody += htmlWriter.h("3", "Productos (sku) pendientes de activar en Vtex");
                mailBody += htmlWriter.table(this.headers, stringSkus.ToArray());
            }

            if (inactivatedSkus.Length > 0)
            {
                List<string[]> stringSkus = new List<string[]>();
                foreach (Sku sku in inactivatedSkus)
                {
                    string[] stringSku = { sku.siesa_id, sku.vtex_id.ToString(), sku.name };
                    stringSkus.Add(stringSku);
                }
                mailBody += htmlWriter.h("3", "Productos (sku) desactivados por el Middleware");
                mailBody += htmlWriter.table(this.headers, stringSkus.ToArray());
            }

            if (notProccecedSkus.Length > 0)
            {
                List<string[]> stringSkus = new List<string[]>();
                foreach (Sku sku in notProccecedSkus)
                {
                    string[] stringSku = { sku.siesa_id, sku.vtex_id.ToString(), sku.name };
                    stringSkus.Add(stringSku);
                }
                mailBody += htmlWriter.h("3", "Productos sin procesar");
                mailBody += htmlWriter.table(this.headers, stringSkus.ToArray());
            }
            mailBody += "</body> </html>";
            this.emailSender.SendEmail(this.emailTitle + " " + this.dateTime.ToString(), mailBody);
        }
    }
}
