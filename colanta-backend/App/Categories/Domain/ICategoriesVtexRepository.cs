namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;
    public interface ICategoriesVtexRepository
    {
        void ChangeEnvironment(string environment);
        Task<Category?> GetCategoryByVtexId(int vtexId);
        Task<Category?> GetCategoryByName(string name);
        Task<Category> GetCategoryById(int id);
        Task<Category> SaveCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<bool> UpdateCategoryState(int vtexId, bool state);

        Task<bool> UpdateCategoryFather(int vtexId, int fatherVtexId);
    }
}
