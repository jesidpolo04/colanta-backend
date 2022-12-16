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
        private string from = "pidecolanta@colanta.com.co";

        public ColantaSender()
        {
            this.smtpClient = new SmtpClient("10.50.0.135");
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
