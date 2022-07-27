namespace colanta_backend.App.Products.Domain
{
    public interface IBrandMustExistMail
    {
        void sendMail(string subject, string templatePath, object model);
    }
}
