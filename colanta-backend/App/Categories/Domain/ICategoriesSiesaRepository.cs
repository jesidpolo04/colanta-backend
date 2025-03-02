namespace colanta_backend.App.Categories.Domain
{
    using System.Threading.Tasks;

    public interface ICategoriesSiesaRepository
    {
        Task<Category[]> GetAllCategories();
    }
}
