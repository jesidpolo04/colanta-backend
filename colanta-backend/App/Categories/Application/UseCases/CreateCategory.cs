namespace colanta_backend.App.Categories.Application
{
    using System.Threading.Tasks;
    using Shared.Domain;
    using Categories.Domain;
    public class CreateCategory
    {
        private CategoriesRepository categoriesRepository;
        public CreateCategory(CategoriesRepository categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<Category> Invoke(Category category)
        {
            return await this.categoriesRepository.saveCategory(category);
        }
    }
}
