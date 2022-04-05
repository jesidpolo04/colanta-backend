namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;

    public interface CategoriesSiesaRepository
    {
        Task<Category[]> getAllCategories();
    }
}
