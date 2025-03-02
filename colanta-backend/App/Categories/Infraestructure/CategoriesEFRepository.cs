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
    using Microsoft.EntityFrameworkCore;
    public class CategoriesEFRepository : ICategoriesRepository
    {
        private readonly ColantaContext _dbContext;
        public CategoriesEFRepository(IConfiguration configuration)
        {
            _dbContext = new ColantaContext(configuration);
        }

        public async Task<Category[]> GetAllCategories()
        {
            EFCategory[] efCategories = await _dbContext.Categories
                                        .Include(c => c.Childs)
                                        .ToArrayAsync();
            List<Category> categories = new List<Category>();
            foreach(EFCategory efCategory in efCategories)
            {
                categories.Add(efCategory.GetCategory());
            }
            return categories.ToArray();
        }

        public async Task<Category[]> GetVtexNullCategories()
        {
            EFCategory[] efCategories = await _dbContext.Categories.Where(category => category.VtexId == null).ToArrayAsync();
            List<Category> categories = new List<Category>();
            foreach(EFCategory efCategory in efCategories)
            {
                categories.Add(efCategory.GetCategory());
            }
            return categories.ToArray();
        }

        public async Task<Category?> GetCategoryBySiesaId(string id)
        {
            var efCategories = _dbContext.Categories
                .Include(c => c.Father)
                .Include(c => c.Childs)
                .ThenInclude(child => child.Father)
                .Where(category => category.SiesaId == id);
                
            if((await efCategories.ToArrayAsync()).Length > 0)
            {
                EFCategory efCategory = await efCategories.FirstAsync();
                return efCategory.GetCategory();
            }
            return null;
        }

        public async Task<Category[]> GetDeltaCategories(Category[] currentCategories)
        {
            List<string> currentIds = new List<string>();
            foreach(Category category in currentCategories)
            {
                currentIds.Add(category.SiesaId);
                foreach(Category child in category.Childs)
                {
                    currentIds.Add(child.SiesaId);
                }
            }
            EFCategory[] efDeltaCategories = await _dbContext.Categories.Where(
                category => !currentIds.Contains(category.SiesaId) && category.IsActive
            ).ToArrayAsync();
            List<Category> categories = new List<Category>();
            foreach (EFCategory efDeltaCategory in efDeltaCategories)
            {
                categories.Add(efDeltaCategory.GetCategory());
            }
            return categories.ToArray();
        }

        public async Task<Category> SaveCategory(Category category)
        {
            EFCategory efCategory = new EFCategory();
            efCategory.SetFromCategory(category);
            _dbContext.Add(efCategory);
            await _dbContext.SaveChangesAsync();
            return await GetCategoryBySiesaId(category.SiesaId);
        }

        public async Task<Category[]> UpdateCategories(Category[] categories)
        {
            foreach(Category category in categories)
            {
                EFCategory efCategory = await _dbContext.Categories.FindAsync(category.Id);
                efCategory.Name = category.Name;
                efCategory.VtexId = category.VtexId;
                efCategory.SiesaId = category.SiesaId;
                efCategory.Business = category.Business;
                efCategory.IsActive = category.IsActive;
            }
            await _dbContext.SaveChangesAsync();
            return categories;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            EFCategory efCategory = await _dbContext.Categories.FindAsync(category.Id);
            efCategory.Name = category.Name;
            efCategory.VtexId = category.VtexId;
            efCategory.SiesaId = category.SiesaId;
            efCategory.Business = category.Business;
            efCategory.IsActive = category.IsActive;

            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category[]> GetVtexCategories()
        {
            EFCategory[] efCategories = await _dbContext.Categories.Where(category => category.VtexId != null).ToArrayAsync();
            List<Category> categories = new List<Category>();
            foreach (EFCategory efCategory in efCategories)
            {
                categories.Add(efCategory.GetCategory());
            }
            return categories.ToArray();
        }
    }
}
