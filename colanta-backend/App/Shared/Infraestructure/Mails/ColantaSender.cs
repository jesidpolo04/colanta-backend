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
            pass = configuration["SmtpPassword"];
            Email.DefaultSender = new SmtpSender(() => GetSmtpClient());
            Email.DefaultRenderer = new RazorRenderer();
        }

        public void SendEmail(string title, string templatePath, object model, string to)
        {
            using (var smtp = GetSmtpClient())
            {
                var sender = new SmtpSender(() => smtp);
                var email = new Email(Email.DefaultRenderer, sender);
                email.SetFrom(this.from, "Middleware Colanta")
                     .To(to)
                     .Subject(title)
                     .UsingTemplateFromFile(templatePath, model, true);
                email.Send();
            }
        }

        public void SendEmailMultiple(string title, string templatePath, object model, List<string> to)
        {
            throw new System.NotImplementedException();
        }

        public void SendEmailWithoutTemplate(string title, string message, string to)
        {
            using (var smtp = GetSmtpClient())
            {
                var sender = new SmtpSender(() => smtp);
                var email = new Email(Email.DefaultRenderer, sender);
                email.SetFrom(this.from, "Middleware Colanta")
                     .To(to)
                     .Subject(title)
                     .Body(message, true);
                email.Send();
            }
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

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };
        }
    }
}
