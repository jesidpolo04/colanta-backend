namespace colanta_backend.App.Categories.Infraestructure
{
    using Categories.Domain;
    using Shared.Domain;
    using System.Collections.Generic;

    public class RenderCategoriesMail : IRenderCategoriesMail
    {
        private EmailSender emailSender;
        private string subject;
        public RenderCategoriesMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "Renderizado de categorías";
        }

        public void sendMail(List<Category> loadedCategories, List<Category> inactivatedCategories, List<Category> failedCategories)
        {
            bool sendMail = false;
            sendMail = loadedCategories.Count > 0 ? true : sendMail;
            sendMail = inactivatedCategories.Count > 0 ? true : sendMail;
            sendMail = failedCategories.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderCategoriesMailModel model = new RenderCategoriesMailModel(loadedCategories, inactivatedCategories, failedCategories);
                this.emailSender.SendEmail(this.subject, "./App/Categories/Infraestructure/Mails/RenderCategoriesMail.cshtml", model);
            }
        }
    }
}
