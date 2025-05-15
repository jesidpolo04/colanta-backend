namespace colanta_backend.App.Categories.Infraestructure
{
    using App.Categories.Domain;
    using App.Shared.Infraestructure;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesEFRepository : ICategoriesRepository
    {
        private readonly IConfiguration _configuration;

        public CategoriesEFRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Category[]> GetAllCategories()
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                EFCategory[] efCategories = await dbContext.Categories
                                        .Include(c => c.Childs)
                                        .ToArrayAsync();
                List<Category> categories = new List<Category>();
                foreach (EFCategory efCategory in efCategories)
                {
                    categories.Add(efCategory.GetCategory());
                }
                return categories.ToArray();
            }
        }

        public async Task<Category[]> GetVtexNullCategories()
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                EFCategory[] efCategories = await dbContext.Categories
                                        .Include(c => c.Childs)
                                        .Where(category => category.VtexId == null)
                                        .ToArrayAsync();
                List<Category> categories = new List<Category>();
                foreach (EFCategory efCategory in efCategories)
                {
                    categories.Add(efCategory.GetCategory());
                }
                return categories.ToArray();
            }
        }

        public async Task<Category?> GetCategoryBySiesaId(string id)
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                EFCategory efCategory = await dbContext.Categories
                    .Include(c => c.Father)
                    .Include(c => c.Childs)
                    .ThenInclude(child => child.Father)
                    .FirstOrDefaultAsync(category => category.SiesaId == id);

                if (efCategory != null)
                {
                    return efCategory.GetCategory();
                }
                return null;
            }
        }

        public async Task<Category[]> GetDeltaCategories(Category[] currentCategories)
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                List<string> currentIds = new List<string>();
                foreach (Category category in currentCategories)
                {
                    currentIds.Add(category.SiesaId);
                    foreach (Category child in category.Childs)
                    {
                        currentIds.Add(child.SiesaId);
                    }
                }
                EFCategory[] efDeltaCategories = await dbContext.Categories.Where(
                    category => !currentIds.Contains(category.SiesaId) && category.IsActive
                ).ToArrayAsync();
                List<Category> categories = new List<Category>();
                foreach (EFCategory efDeltaCategory in efDeltaCategories)
                {
                    categories.Add(efDeltaCategory.GetCategory());
                }
                return categories.ToArray();
            }
        }

        public async Task<Category> SaveCategory(Category category)
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                EFCategory efCategory = new EFCategory();
                efCategory.SetFromCategory(category);
                //Attach the father category if it exists
                if (efCategory.Father is not null)
                {
                    dbContext.Attach(efCategory.Father);
                }
                dbContext.Add(efCategory);
                await dbContext.SaveChangesAsync();
                return await GetCategoryBySiesaId(category.SiesaId);
            }
        }

        public async Task<Category[]> UpdateCategories(Category[] categories)
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                foreach (Category category in categories)
                {
                    EFCategory efCategory = await dbContext.Categories.FindAsync(category.Id);
                    efCategory.Name = category.Name;
                    efCategory.VtexId = category.VtexId;
                    efCategory.SiesaId = category.SiesaId;
                    efCategory.Business = category.Business;
                    efCategory.IsActive = category.IsActive;
                }
                await dbContext.SaveChangesAsync();
                return categories;
            }
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                EFCategory efCategory = await dbContext.Categories.FindAsync(category.Id);
                efCategory.Name = category.Name;
                efCategory.VtexId = category.VtexId;
                efCategory.SiesaId = category.SiesaId;
                efCategory.Business = category.Business;
                efCategory.IsActive = category.IsActive;

                await dbContext.SaveChangesAsync();
                return category;
            }
        }

        public async Task<Category[]> GetVtexCategories()
        {
            using (var dbContext = new ColantaContext(_configuration))
            {
                EFCategory[] efCategories = await dbContext.Categories.Where(category => category.VtexId != null).ToArrayAsync();
                List<Category> categories = new List<Category>();
                foreach (EFCategory efCategory in efCategories)
                {
                    categories.Add(efCategory.GetCategory());
                }
                return categories.ToArray();
            }
        }
    }
}
