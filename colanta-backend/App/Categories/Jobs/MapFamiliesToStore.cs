namespace colanta_backend.App.Categories.Jobs
{
    using Categories.Domain;
    using System.Threading.Tasks;
    public class MapFamiliesToStore
    {
        private ICategoriesVtexRepository vtexRepository;
        private ICategoriesRepository localRepository;

        public MapFamiliesToStore(ICategoriesVtexRepository vtexRepository, ICategoriesRepository localRepository)
        {
            this.vtexRepository = vtexRepository;
            this.localRepository = localRepository;
        }

        public async Task Invoke()
        {
            Category[] localCategories = await this.localRepository.GetAllCategories();
            foreach (Category category in localCategories) 
            {
                if(category.Father == null)
                {
                    if (category.Business == "mercolanta") await this.vtexRepository.UpdateCategoryFather((int)category.VtexId, MercolantaCategory.vtexId);
                    if (category.Business == "agrocolanta") await this.vtexRepository.UpdateCategoryFather((int)category.VtexId, AgrocolantaCategory.vtexId);
                }
            }
        }
    }
}
