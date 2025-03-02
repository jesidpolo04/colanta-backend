

namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;
    public interface ICategoriesRepository
    {
        Task<Category[]> GetAllCategories();
        Task<Category[]> GetVtexNullCategories();
        Task<Category[]> GetVtexCategories();
        Task<Category[]> GetDeltaCategories(Category[] currentCategories);
        Task<Category?> GetCategoryBySiesaId(string id);
        Task<Category> SaveCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<Category[]> UpdateCategories(Category[] categories);
    }
}
