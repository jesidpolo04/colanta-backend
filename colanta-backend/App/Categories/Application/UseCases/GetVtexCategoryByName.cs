namespace colanta_backend.App.Categories.Application
{
    using Categories.Domain;
    using Shared.Domain;
    using System.Threading.Tasks;
    public class GetVtexCategoryByName
    {
        private CategoriesVtexRepository vtexRepository;

        public GetVtexCategoryByName(CategoriesVtexRepository vtexRepository)
        {
            this.vtexRepository = vtexRepository;
        }

        public async Task<Category?> Invoke(string name)
        {
            return await this.vtexRepository.getCategoryByName(name);
        }
    }
}
