namespace colanta_backend.App.Products.Domain
{
    public interface ICategoryMustExistMail
    {
        void sendMail(string subject, string templatePath, object model);
    }
}
