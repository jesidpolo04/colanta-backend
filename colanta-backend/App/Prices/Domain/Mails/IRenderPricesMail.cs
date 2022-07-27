namespace colanta_backend.App.Prices.Domain
{
    public interface IRenderPricesMail
    {
        void sendMail(string subject, string templatePath, object model);

    }
}
