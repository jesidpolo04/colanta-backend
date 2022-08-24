namespace colanta_backend.App.Shared.Infraestructure
{
    using Shared.Domain;
    using System.Threading.Tasks;
    using System.Net.Mail;
    using System.Net;
    using System;
    using Microsoft.Extensions.Configuration;
    using FluentEmail.Core;
    using FluentEmail.Razor;
    using FluentEmail.Smtp;
    using System.Collections.Generic;

    public class GmailSender : EmailSender, IDisposable
    {
        private SmtpClient smtpClient;
        private string password;
        private string user;

        private string from = "wilsonflorez184444z@gmail.com";
        
        public GmailSender(IConfiguration configuration)
        {
            this.password = configuration["SmtpPassword"];
            this.user = configuration["SmtpUser"];

            this.smtpClient = new SmtpClient(configuration["SmtpServer"]);
            this.smtpClient.Port = Int32.Parse(configuration["SmtpPort"]);
            this.smtpClient.EnableSsl = true;
            this.smtpClient.UseDefaultCredentials = false;
            this.smtpClient.Credentials = new NetworkCredential(this.user, this.password);
            Email.DefaultSender = new SmtpSender(this.smtpClient);
            Email.DefaultRenderer = new RazorRenderer();
        }

        public void Dispose()
        {
            this.smtpClient?.Dispose();
        }

        public void SendEmail(string title, string templatePath, object model, string to )
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
            throw new NotImplementedException();
        }
    }
}
