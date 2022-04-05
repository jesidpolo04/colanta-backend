namespace colanta_backend.App.Categories.Application
{
    using System.Threading.Tasks;
    using Categories.Domain;
    public class GetAllSiesaCategories
    {
        private CategoriesSiesaRepository repository;

        public GetAllSiesaCategories(CategoriesSiesaRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category[]> Invoke()
        {
            return await this.repository.getAllCategories();
        }
    }
}
