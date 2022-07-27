namespace colanta_backend.App.Products.Domain
{
    using System;
    public class BrandMustExistException : Exception
    {
        public Product product;
        public BrandMustExistException(string message, Product product):base(message)
        {
            this.product = product;
        }
    }
}
