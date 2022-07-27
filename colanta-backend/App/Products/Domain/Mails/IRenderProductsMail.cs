namespace colanta_backend.App.Products.Domain
{
    public interface IRenderProductsMail
    {
        void sendMail(string subject, string templatePath, object model);
    }
}
