namespace colanta_backend.App.Categories.Domain
{
    public interface IRenderCategoriesMail
    {
        void sendMail(string subject, string templatePath, object model);

    }
}
