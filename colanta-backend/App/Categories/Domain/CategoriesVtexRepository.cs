namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;
    public interface CategoriesVtexRepository
    {
        Task<Category> getCategoryByVtexId(int vtexId);
        Task<Category> getCategoryById(int id);
        Task<Category> saveCategory(Category category);
        Task<Category> updateCategory(Category category);
    }
}
