namespace colanta_backend.App.Products.Domain
{
    using System;
    public class CategoryMustExistException : Exception
    {
        public Product product;
        public CategoryMustExistException(string message, Product product) : base(message)
        {
            this.product = product;
        }
    }
}
