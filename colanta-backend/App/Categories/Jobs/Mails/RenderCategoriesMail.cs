namespace colanta_backend.App.Categories.Jobs
{
    using Shared.Domain;
    using Categories.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderCategoriesMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        private string[] headers = { "siesa_id", "vtex_id", "nombre" };
        public string emailTitle = "Renderizado de Categorías";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;

        public RenderCategoriesMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Category[] loadCategories, Category[] inactiveCategories, Category[] failedLoadCategories, Category[] notProccecedCategories, Category[] inactivatedCategories)
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

            if (loadCategories.Length > 0)
            {
                List<string[]> stringCategories = new List<string[]>();
                foreach (Category category in loadCategories)
                {
                    string[] stringBrand = { category.siesa_id, category.vtex_id.ToString(), category.name };
                    stringCategories.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Categorías cargadas a Vtex");
                mailBody += htmlWriter.table(this.headers, stringCategories.ToArray());
            }

            if (failedLoadCategories.Length > 0)
            {
                List<string[]> stringCategories = new List<string[]>();
                foreach (Category category in failedLoadCategories)
                {
                    string[] stringBrand = { category.siesa_id, category.vtex_id.ToString(), category.name };
                    stringCategories.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Categorías que no se cargaron en Vtex (errores)");
                mailBody += htmlWriter.table(this.headers, stringCategories.ToArray());
            }

            if (inactiveCategories.Length > 0)
            {
                List<string[]> stringCategories = new List<string[]>();
                foreach (Category category in inactiveCategories)
                {
                    string[] stringBrand = { category.siesa_id, category.vtex_id.ToString(), category.name };
                    stringCategories.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Categorías pendientes de activar en Vtex");
                mailBody += htmlWriter.table(this.headers, stringCategories.ToArray());
            }

            if (inactivatedCategories.Length > 0)
            {
                List<string[]> stringCategories = new List<string[]>();
                foreach (Category category in inactivatedCategories)
                {
                    string[] stringBrand = { category.siesa_id, category.vtex_id.ToString(), category.name };
                    stringCategories.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Categorías desactivadas por el Middleware");
                mailBody += htmlWriter.table(this.headers, stringCategories.ToArray());
            }

            if (notProccecedCategories.Length > 0)
            {
                List<string[]> stringCategories = new List<string[]>();
                foreach (Category category in notProccecedCategories)
                {
                    string[] stringBrand = { category.siesa_id, category.vtex_id.ToString(), category.name };
                    stringCategories.Add(stringBrand);
                }
                mailBody += htmlWriter.h("3", "Categorías desactivadas por el Middleware");
                mailBody += htmlWriter.table(this.headers, stringCategories.ToArray());
            }
            mailBody += "</body> </html>";
            this.emailSender.SendEmail(this.emailTitle + " " + this.dateTime.ToString(), mailBody);
        }
    }
}
