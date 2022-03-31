namespace colanta_backend.App.Shared.Infraestructure
{
    using Shared.Domain;
    using System.Net.Mail;
    using System.Net;

    public class GmailSender : EmailSender
    {
        private SmtpClient smtpClient;
        private string from = "jesing482@gmail.com";
        private string to = "jesdady482@gmail.com";
        private string password = "1020999476";
        public GmailSender()
        {
            this.smtpClient = new SmtpClient("smtp.gmail.com");
            this.smtpClient.EnableSsl = true;
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.Port = 587;
            this.smtpClient.Credentials = new NetworkCredential(this.from, this.password);
        }
        public void SendEmail(string message)
        {
            MailMessage mail = new MailMessage(this.from, this.to, "Colanta Middleware Process", message);
            
            this.smtpClient.Send(mail);
        }
    }
}
