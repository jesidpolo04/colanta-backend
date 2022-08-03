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
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private CategoriesSiesaRepository siesaRepository;

        public ActivateAllCategories(CategoriesRepository localRepository, CategoriesVtexRepository vtexRepository, CategoriesSiesaRepository siesaRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task Invoke()
        {
            Category[] allCategories = await this.localRepository.getAllCategories();
            foreach (Category category in allCategories)
            {
                category.isActive = true;
                vtexRepository.updateCategoryState((int)category.vtex_id, true).Wait();
                await localRepository.updateCategory(category);
            }
        }
    }
}
