namespace colanta_backend.App.Products.Domain
{
    using System;
    public class InvalidBrandException : Exception
    {
        public Product product;
        public InvalidBrandException(string message, Product product):base(message)
        {
            this.product = product;
        }
    }
}
