namespace colanta_backend.App.Inventory.Jobs
{
    using Shared.Domain;
    using Inventory.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderInventoriesMail
    {
        private HtmlWriter htmlWriter;
        private EmailSender emailSender;
        public string emailTitle = "Renderizado de Inventario";
        public string emailSubtitle = "Middleware Colanta";
        public DateTime dateTime;

        public RenderInventoriesMail(EmailSender emailSender)
        {
            this.htmlWriter = new HtmlWriter();
            this.emailSender = emailSender;
            this.dateTime = DateTime.Now;
        }

        public void sendMail(Inventory[] failedLoadInventories)
        {
            bool sendEmail = false;
            string body = "";

            if (failedLoadInventories.Length > 0)
            {
                sendEmail = true;
                body += htmlWriter.h("4", "Inventarios que no pudieron ser actualizados en VTEX") + "\n";
                List<string> failedLoadInventoriesInfo = new List<string>();
                foreach (Inventory failedLoadInventory in failedLoadInventories)
                {
                    failedLoadInventoriesInfo.Add($"{failedLoadInventory.sku.name} ({failedLoadInventory.sku.siesa_id}), centro de operación: {failedLoadInventory.warehouse.name} ({failedLoadInventory.warehouse.siesa_id}), cantidad: {failedLoadInventory.quantity}");
                }
                body += htmlWriter.ul(failedLoadInventoriesInfo.ToArray());
                body += "\n";
            }

            if (sendEmail)
            {
                this.emailSender.SendEmail(this.emailTitle, body);
            }
        }
    }
}
