namespace colanta_backend.App.Products.Domain
{
    using System;
    public class InvalidCategoryException : Exception
    {
        public Product product;
        public InvalidCategoryException(string message, Product product) : base(message)
        {
            this.product = product;
        }
    }
}
