namespace colanta_backend.App.Brands.Jobs
{
    using Shared.Domain;
    using Brands.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderBrandsMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        private string[] headers = {"siesa_id", "vtex_id", "nombre"};
        public string emailTitle = "Renderizado de Marcas";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;
        public RenderBrandsMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }
        public void sendMail(Brand[] loadBrands, Brand[] inactiveBrands, Brand[] failedLoadBrands, Brand[] notProccecedBrands, Brand[] inactivatedBrands)
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

            if (loadBrands.Length > 0)
            {
                List<string[]> stringBrands = new List<string[]>();
                foreach(Brand brand in loadBrands)
                {
                    string[] stringBrand = { brand.id_siesa, brand.id_vtex.ToString(), brand.name };
                    stringBrands.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Marcas cargadas a Vtex");
                mailBody += htmlWriter.table(this.headers, stringBrands.ToArray());
            }

            if (failedLoadBrands.Length > 0)
            {
                List<string[]> stringBrands = new List<string[]>();
                foreach (Brand brand in failedLoadBrands)
                {
                    string[] stringBrand = { brand.id_siesa, brand.id_vtex.ToString(), brand.name };
                    stringBrands.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Marcas que no se cargaron en Vtex (errores)");
                mailBody += htmlWriter.table(this.headers, stringBrands.ToArray());
            }

            if (inactiveBrands.Length > 0)
            {
                List<string[]> stringBrands = new List<string[]>();
                foreach (Brand brand in inactiveBrands)
                {
                    string[] stringBrand = { brand.id_siesa, brand.id_vtex.ToString(), brand.name };
                    stringBrands.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Marcas pendientes de activar en Vtex");
                mailBody += htmlWriter.table(this.headers, stringBrands.ToArray());
            }

            if (inactivatedBrands.Length > 0)
            {
                List<string[]> stringBrands = new List<string[]>();
                foreach (Brand brand in inactivatedBrands)
                {
                    string[] stringBrand = { brand.id_siesa, brand.id_vtex.ToString(), brand.name };
                    stringBrands.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Marcas desactivadas por el Middleware");
                mailBody += htmlWriter.table(this.headers, stringBrands.ToArray());
            }

            if (notProccecedBrands.Length > 0)
            {
                List<string[]> stringBrands = new List<string[]>();
                foreach (Brand brand in notProccecedBrands)
                {
                    string[] stringBrand = { brand.id_siesa, brand.id_vtex.ToString(), brand.name };
                    stringBrands.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Marcas desactivadas por el Middleware");
                mailBody += htmlWriter.table(this.headers, stringBrands.ToArray());
            }
            mailBody += "</body> </html>";
            this.emailSender.SendEmail(this.emailTitle + " " + this.dateTime.ToString(), mailBody);
        }
    }
}
