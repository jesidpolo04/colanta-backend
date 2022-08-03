namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using CustomerCredit.Domain;
    using System.Collections.Generic;
    using Shared.Domain;
    public class RenderAccountsMail : IRenderAccountsMail
    {
        private EmailSender emailSender;
        private string subject = "Renderizado de cupos";

        public RenderAccountsMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
        }
        public void sendMail(List<CreditAccount> loadedAccounts, List<CreditAccount> updatedAccounts, List<CreditAccount> failedAccounts)
        {
            bool sendMail = false;
            sendMail = loadedAccounts.Count > 0 ? true : sendMail;
            sendMail = updatedAccounts.Count > 0 ? true : sendMail;
            sendMail = failedAccounts.Count > 0 ? true : sendMail;
            if (sendMail)
            {
                RenderCreditAccountMailModel model = new RenderCreditAccountMailModel(loadedAccounts, updatedAccounts, failedAccounts);
                this.emailSender.SendEmail(this.subject, "./App/CustomerCredit/Infraestructure/Mails/RenderAccountsMail.cshtml", model);
            }
        }
    }
}
