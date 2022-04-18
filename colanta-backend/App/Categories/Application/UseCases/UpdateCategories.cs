namespace colanta_backend.App.Categories.Application
{
    using System;
    using System.Threading.Tasks;
    using Categories.Domain;

    public class UpdateCategories
    {
        private CategoriesRepository repository;

        public UpdateCategories(CategoriesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category[]> Invoke(Category[] categories)
        {
            return await this.repository.updateCategories(categories);
        }
    }
}
