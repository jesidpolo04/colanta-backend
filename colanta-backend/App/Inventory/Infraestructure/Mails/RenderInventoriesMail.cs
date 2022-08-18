namespace colanta_backend.App.Inventory.Infraestructure
{
    using Inventory.Domain;
    using Shared.Domain;
    using System.Collections.Generic;
    public class RenderInventoriesMail : IRenderInventoriesMail
    {
        private EmailSender emailSender;
        private string subject;

        public RenderInventoriesMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "Renderizado de inventarios";
        }

        public void sendMail(List<Inventory> loadedInventories, List<Inventory> updatedInventories, List<Inventory> failedInventories)
        {
            bool sendMail = false;
            sendMail = loadedInventories.Count > 0 ? true : sendMail;
            sendMail = updatedInventories.Count > 0 ? true : sendMail;
            sendMail = failedInventories.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderInventoriesMailModel model = new RenderInventoriesMailModel(loadedInventories, updatedInventories, failedInventories);
                this.emailSender.SendEmail(this.subject, "./App/Inventory/Infraestructure/Mails/RenderInventoriesMail.cshtml", model, EmailAddresses.Administrative);
            }
        }
    }
}
