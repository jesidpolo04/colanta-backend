namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;
    using Categories.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System.Text.Json.Serialization;
    public class ActivateAllCategories
    {
        private ICategoriesRepository localRepository;
        private ICategoriesVtexRepository vtexRepository;
        private ICategoriesSiesaRepository siesaRepository;

        public ActivateAllCategories(ICategoriesRepository localRepository, ICategoriesVtexRepository vtexRepository, ICategoriesSiesaRepository siesaRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task Invoke()
        {
            Category[] allCategories = await this.localRepository.GetAllCategories();
            foreach (Category category in allCategories)
            {
                category.IsActive = true;
                vtexRepository.UpdateCategoryState((int)category.VtexId, true).Wait();
                await localRepository.UpdateCategory(category);
            }
        }
    }
}
