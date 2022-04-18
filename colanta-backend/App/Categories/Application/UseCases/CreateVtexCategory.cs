namespace colanta_backend.App.Categories.Application
{
    using System.Threading.Tasks;
    using Shared.Domain;
    using Categories.Domain;
    using System;

    public class CreateVtexCategory
    {
        private CategoriesVtexRepository categoriesVtexRepository;
        private int _numberOfTry = 5;
        public CreateVtexCategory(CategoriesVtexRepository categoriesVtexRepository)
        {
            this.categoriesVtexRepository = categoriesVtexRepository;
        }

        public async Task<Category> Invoke(Category category)
        {
            try
            {
                Category? vtexCategory = await this.categoriesVtexRepository.getCategoryByName(category.name);
                if(vtexCategory == null)
                {
                    return await this.categoriesVtexRepository.saveCategory(category);
                }
                return vtexCategory;
            }
            catch(Exception exception)
            {
                for (int i = 1; i <= this._numberOfTry; i++)
                {
                    try
                    {
                        Category? vtexCategory = await this.categoriesVtexRepository.getCategoryByName(category.name);
                        if (vtexCategory == null)
                        {
                            return await this.categoriesVtexRepository.saveCategory(category);
                        }
                        return vtexCategory;
                    }
                    catch
                    {
                        if (i == this._numberOfTry)
                        {
                            throw exception;
                        }
                    }
                }
                return null;
            }
        }

    }
}
