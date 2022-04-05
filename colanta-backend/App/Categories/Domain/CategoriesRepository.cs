

namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;
    public interface CategoriesRepository
    {
        Task<Category[]> getAllCategories();
        Task<Category[]> getDeltaCategories(Category[] currentCategories);
        Task<Category?> getCategoryBySiesaId(string id);
        Task<Category> saveCategory(Category category);
        Task<Category> updateCategory(Category category);
        Task<Category> updateCategories(Category[] categories);

    }
}
