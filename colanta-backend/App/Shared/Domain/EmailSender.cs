namespace colanta_backend.App.Shared.Domain
{
    using System.Net.Mail;
    using System.Threading.Tasks;
    public interface EmailSender
    {
        public void SendEmail(string title, string templatePath, object model, string to);
    }
}
