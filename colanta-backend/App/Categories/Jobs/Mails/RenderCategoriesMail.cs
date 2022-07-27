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
        public string emailTitle = "Renderizado de Categorías";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;

        public RenderCategoriesMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Category[] inactiveCategories, Category[] failedLoadCategories, Category[] loadCategories)
        {
            bool sendEmail = false;
            string body = "";

            if (loadCategories.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Categorías creadas en VTEX") + "\n";
                List<string> loadCategoriesInfo = new List<string>();
                foreach (Category loadCategory in loadCategories)
                {
                    loadCategoriesInfo.Add($"{loadCategory.name}, con SIESA id: {loadCategory.siesa_id} y VTEX id: {loadCategory.vtex_id}");
                }
                body += htmlWriter.ul(loadCategoriesInfo.ToArray());
                body += "\n";
            }

            if (inactiveCategories.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Categorías Inactivas en VTEX") + "\n";
                List<string> inactiveCategoriesInfo = new List<string>();
                foreach (Category inactiveCategory in inactiveCategories)
                {
                    inactiveCategoriesInfo.Add($"{inactiveCategory.name}, con SIESA id: {inactiveCategory.siesa_id} y VTEX id: {inactiveCategory.vtex_id}");
                }
                body += htmlWriter.ul(inactiveCategoriesInfo.ToArray());
                body += "\n";
            }

            if (failedLoadCategories.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Categorías que fallaron al cargarse a VTEX") + "\n";
                List<string> failedLoadCategoriesInfo = new List<string>();
                foreach (Category failedLoadCategory in failedLoadCategories)
                {
                    failedLoadCategoriesInfo.Add($"{failedLoadCategory.name}, con SIESA id: {failedLoadCategory.siesa_id}");
                }
                body += htmlWriter.ul(failedLoadCategoriesInfo.ToArray());
                body += "\n";
            }

            if (sendEmail)
            {
                //this.emailSender.SendEmail(this.emailTitle, body);
            }
        }
    }
}
