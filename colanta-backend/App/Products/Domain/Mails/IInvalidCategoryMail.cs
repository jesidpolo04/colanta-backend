namespace colanta_backend.App.Products.Domain
{
    using Products.Domain;
    public interface IInvalidCategoryMail
    {
        void sendMail(InvalidCategoryException exception);
    }
}
