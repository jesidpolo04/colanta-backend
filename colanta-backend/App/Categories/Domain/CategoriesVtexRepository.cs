namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;
    public interface CategoriesVtexRepository
    {
        void changeEnvironment(string environment);
        Task<Category?> getCategoryByVtexId(int vtexId);
        Task<Category?> getCategoryByName(string name);
        Task<Category> getCategoryById(int id);
        Task<Category> saveCategory(Category category);
        Task<Category> updateCategory(Category category);
        Task<Category> updateCategoryState(int vtexId, bool state);
    }
}
