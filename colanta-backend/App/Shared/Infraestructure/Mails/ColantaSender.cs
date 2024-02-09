namespace colanta_backend.App.Shared.Infraestructure
{
    using Shared.Domain;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using FluentEmail.Core;
    using FluentEmail.Razor;
    using FluentEmail.Smtp;
    using Microsoft.Extensions.Configuration;

    public class ColantaSender : EmailSender
    {
        private SmtpClient smtpClient;
        private readonly string from;
        private readonly int port;
        private readonly string host;
        private readonly string user;
        private readonly string pass;

        public ColantaSender(IConfiguration configuration)
        {
            from = configuration["SmtpUser"];
            port = int.Parse(configuration["SmtpPort"]);
            host = configuration["SmtpServer"];
            user = configuration["SmtpUser"];
            pass = configuration["SmtpPass"];
            this.smtpClient = new SmtpClient(this.host);
            this.smtpClient.Port = this.port;
            this.smtpClient.Credentials = new NetworkCredential(this.user, this.pass);
            this.smtpClient.EnableSsl = true;
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
                .SendAsync().Wait();
        }

        public void SendEmailMultiple(string title, string templatePath, object model, List<string> to)
        {
            throw new System.NotImplementedException();
        }

        public void sendEmailWithoutTemplate(string title, string message, string to)
        {
            Email
               .From(this.from, "Middleware Colanta")
               .To(to)
               .Subject(title)
               .Body(message)
               .Send();
        }

        public void SendHelloWorld()
        {
            Email
               .From(this.from, "Middleware Colanta")
               .To("jesdady482@gmail.com;jesidpolo04@gmail.com")
               .Subject("Hola mundo")
               .Body("Hola mundo desde Colanta SMTP")
               .Send();
        }
    }
}
