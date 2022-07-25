namespace colanta_backend.App.Shared.Domain
{
    using System;
    using Products.Domain;
    public class BrandMustExistException : Exception
    {
        public Product product;
        public BrandMustExistException(string message, Product product):base(message){
            this.product = product;
        }
    }
}
