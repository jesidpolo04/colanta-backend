namespace colanta_backend.App.Categories.Infraestructure
{
    using App.Categories.Domain;
    using App.Shared.Infraestructure;
    using App.Shared.Application;
    using App.Brands.Infraestructure;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    public class CategoriesEFRepository : CategoriesRepository
    {
        private ColantaContext dbContext;
        public CategoriesEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<Category[]> getAllCategories()
        {
            EFCategory[] efCategories = this.dbContext.Categories.ToArray();
            List<Category> categories = new List<Category>();
            foreach(EFCategory efCategory in efCategories)
            {
                categories.Add(efCategory.getCategoryFromEFCategory());
            }
            return categories.ToArray();
        }

        public async Task<Category?> getCategoryBySiesaId(string id)
        {
            var efCategories = this.dbContext.Categories.Where(category => category.siesa_id == id);
            if(efCategories.ToArray().Length > 0)
            {
                return efCategories.First().getCategoryFromEFCategory();
            }
            return null;
        }

        public async Task<Category[]> getDeltaCategories(Category[] currentCategories)
        {
            List<string> currentIds = new List<string>();
            foreach(Category category in currentCategories)
            {
                currentIds.Add(category.siesa_id);
                foreach(Category child in category.childs)
                {
                    currentIds.Add(child.siesa_id);
                }
            }
            EFCategory[] efDeltaCategories = this.dbContext.Categories.Where(category => !currentIds.Contains(category.siesa_id) && category.isActive == true).ToArray();
            List<Category> categories = new List<Category>();
            foreach (EFCategory efDeltaCategory in efDeltaCategories)
            {
                categories.Add(efDeltaCategory.getCategoryFromEFCategory());
            }
            return categories.ToArray();
        }

        public async Task<Category> saveCategory(Category category)
        {
            EFCategory efCategory = new EFCategory();
            efCategory.setEfCategoryFromCategory(category);
            this.dbContext.Add(efCategory);
            this.dbContext.SaveChanges();
            efCategory = null;
            return await this.getCategoryBySiesaId(category.siesa_id);
        }

        public async Task<Category[]> updateCategories(Category[] categories)
        {
            foreach(Category category in categories)
            {
                EFCategory efCategory = this.dbContext.Categories.Find(category.id);
                efCategory.name = category.name;
                efCategory.vtex_id = category.vtex_id;
                efCategory.siesa_id = category.siesa_id;
                efCategory.business = category.business;
                efCategory.isActive = category.isActive;
            }
            dbContext.SaveChanges();
            return categories;
        }

        public async Task<Category> updateCategory(Category category)
        {
            EFCategory efCategory = this.dbContext.Categories.Find(category.id);
            efCategory.name = category.name;
            efCategory.vtex_id = category.vtex_id;
            efCategory.siesa_id = category.siesa_id;
            efCategory.business = category.business;
            efCategory.isActive = category.isActive;

            this.dbContext.SaveChanges();
            return category;
        }
    }
}
