namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using CustomerCredit.Domain;
    using Shared.Domain;
    using System.Collections.Generic;
    public class InvalidAccountsMail : IInvalidAccountsMail
    {
        private EmailSender emailSender;
        private string subject;
        private string template;
        public InvalidAccountsMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.subject = "(Cupo lacteo) Cuentas inválidas";
            this.template = "./App/CustomerCredit/Infraestructure/Mails/InvalidAccountsMail.cshtml";
        }
        public void sendMail(List<InvalidAccountException> exceptions)
        {
            if(exceptions.Count > 0)
            {
                InvalidAccountsMailModel model = new InvalidAccountsMailModel(exceptions);
                this.emailSender.SendEmail(subject, template, model);
            }
        }
    }
}
