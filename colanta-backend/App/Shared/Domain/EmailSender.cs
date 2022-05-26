namespace colanta_backend.App.Shared.Domain
{
    using System.Net.Mail;
    public interface EmailSender
    {
        public void SendEmail(string title, string message);
    }
}
