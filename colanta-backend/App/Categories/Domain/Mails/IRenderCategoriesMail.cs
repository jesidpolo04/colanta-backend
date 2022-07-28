namespace colanta_backend.App.Categories.Domain
{
    using Categories.Domain;
    using System.Collections.Generic;
    public interface IRenderCategoriesMail
    {
        void sendMail(List<Category> loadedCategories, List<Category> inactivatedCategories, List<Category> failedCategories);
    }
}
