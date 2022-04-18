namespace colanta_backend.App.Categories.Application
{
    using System;
    using System.Threading.Tasks;
    using Categories.Domain;
   
    public class UpdateCategory
    {
        private CategoriesRepository repository;

        public UpdateCategory(CategoriesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category> Invoke(Category category)
        {
            return await this.repository.updateCategory(category);
        }
    }
}
