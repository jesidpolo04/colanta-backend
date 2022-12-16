namespace colanta_backend.App.Shared.Domain
{
    using System.Collections.Generic;
    public interface EmailSender
    {
        public void sendEmailWithoutTemplate(string title, string message, string to);
        public void SendEmail(string title, string templatePath, object model, string to);
        public void SendEmailMultiple(string title, string templatePath, object model, List<string> to);
    }
}
