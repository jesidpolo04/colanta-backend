namespace colanta_backend.App.Shared.Domain
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public interface EmailSender
    {
        public void SendEmail(string title, string templatePath, object model, string to);
        public void SendEmailMultiple(string title, string templatePath, object model, List<string> to);
    }
}
