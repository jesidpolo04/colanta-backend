namespace colanta_backend.App.Categories.Domain
{
    using System;
    public class ChildCategoryAlreadyExistException : Exception
    {
        public Category category;
        public Category child;
        public ChildCategoryAlreadyExistException(string message, Category category, Category child)
            :base(message)
        {
            this.category = category;
            this.child = child;
        }
    }
}
