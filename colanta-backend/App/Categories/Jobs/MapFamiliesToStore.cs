namespace colanta_backend.App.Categories.Jobs
{
    using Categories.Domain;
    using System.Threading.Tasks;
    public class MapFamiliesToStore
    {
        private CategoriesVtexRepository vtexRepository;
        private CategoriesRepository localRepository;

        public MapFamiliesToStore(CategoriesVtexRepository vtexRepository, CategoriesRepository localRepository)
        {
            this.vtexRepository = vtexRepository;
            this.localRepository = localRepository;
        }

        public async Task Invoke()
        {
            Category[] localCategories = await this.localRepository.getAllCategories();
            foreach (Category category in localCategories) 
            {
                if(category.father == null)
                {
                    if (category.business == "mercolanta") await this.vtexRepository.updateCategoryFather((int)category.vtex_id, MercolantaCategory.vtexId);
                    if (category.business == "agrocolanta") await this.vtexRepository.updateCategoryFather((int)category.vtex_id, AgrocolantaCategory.vtexId);
                }
            }
        }
    }
}
