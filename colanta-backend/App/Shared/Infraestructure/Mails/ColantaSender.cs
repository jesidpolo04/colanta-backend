namespace colanta_backend.App.Shared.Infraestructure
{
    using Shared.Domain;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using FluentEmail.Core;
    using FluentEmail.Razor;
    using FluentEmail.Smtp;

    public class ColantaSender : EmailSender
    {
        private SmtpClient smtpClient;
        private string password;
        private string user;
        private string from = "wilsonflorez184444z@gmail.com";

        public ColantaSender()
        {
            this.password = "Colanta$2025";
            this.user = @"med\pidecolanta";

            this.smtpClient = new SmtpClient("http://10.50.0.135");
            this.smtpClient.EnableSsl = false;
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.Credentials = new NetworkCredential(this.user, this.password);
            Email.DefaultSender = new SmtpSender(this.smtpClient);
            Email.DefaultRenderer = new RazorRenderer();
        }

        public void SendEmail(string title, string templatePath, object model, string to)
        {
            Email
                .From(this.from, "Middleware Colanta")
                .To(to)
                .Subject(title)
                .UsingTemplateFromFile(templatePath, model, true)
                .Send();
        }

        public void SendEmailMultiple(string title, string templatePath, object model, List<string> to)
        {
            throw new System.NotImplementedException();
        }
    }
}
