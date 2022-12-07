namespace colanta_backend.App.Prices.Domain
{
    using Products.Domain;
    public interface INotifyMissingPriceMail
    {
        void sendMail(Sku sku);
    }
}
