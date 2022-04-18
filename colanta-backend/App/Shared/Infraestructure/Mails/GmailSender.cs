namespace colanta_backend.App.Shared.Infraestructure
{
    using Shared.Domain;
    using System.Net.Mail;
    using System.Net;
    using System;
    using Microsoft.Extensions.Configuration;

    public class GmailSender : EmailSender
    {
        private SmtpClient smtpClient;
        private string password;
        private string user;

        private string from = "jesing482@gmail.com";
        private string to = "jesdady482@gmail.com";
        
        public GmailSender(IConfiguration configuration)
        {
            this.password = configuration["SmtpPassword"];
            this.user = configuration["SmtpUser"];

            this.smtpClient = new SmtpClient(configuration["SmtpServer"]);
            this.smtpClient.Port = configuration.GetValue<int>("SmtpPort");
            this.smtpClient.EnableSsl = true;
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.Credentials = new NetworkCredential(this.user, this.password);
        }
        public void SendEmail(string message)
        {
            MailMessage mail = new MailMessage(this.from, this.to, "Colanta Middleware Process", message);
            mail.IsBodyHtml = true;
            
            this.smtpClient.Send(mail);
        }
    }
}
