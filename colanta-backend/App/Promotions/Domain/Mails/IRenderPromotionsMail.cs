namespace colanta_backend.App.Promotions.Domain
{
    public interface IRenderPromotionsMail
    {
        void sendMail(string subject, string templatePath, object model);

    }
}
