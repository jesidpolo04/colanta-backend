namespace colanta_backend.App.Categories.Application
{
    using System.Threading.Tasks;
    using Categories.Domain;
    public class GetCategoryBySiesaId
    {
        private CategoriesRepository repository;

        public GetCategoryBySiesaId(CategoriesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category?> Invoke(string siesaId)
        {
            return await this.repository.getCategoryBySiesaId(siesaId);
        }
    }
}
