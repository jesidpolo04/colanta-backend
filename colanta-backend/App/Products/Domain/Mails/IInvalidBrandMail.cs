namespace colanta_backend.App.Products.Domain
{
    using Products.Domain;
    public interface IInvalidBrandMail
    {
       void sendMail(InvalidBrandException exception);
    }
}
