namespace colanta_backend.App.Categories.Application
{
    using Categories.Domain;
    using System.Threading.Tasks;

    public class GetDeltaCategories
    {
        private CategoriesRepository repository;

        public GetDeltaCategories(CategoriesRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category[]> Invoke(Category[] currentCategories)
        {
            return await this.repository.getDeltaCategories(currentCategories);
        }
    }
}
