namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading.Tasks;
    using Categories.Domain;
    using Categories.Application;
    using Shared.Domain;
    using Shared.Application;
    public class RenderCategories
    {
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private CategoriesSiesaRepository siesaRepository;
        public RenderCategories(
            CategoriesRepository categoriesLocalRepository, 
            CategoriesVtexRepository categoriesVtexRepository, 
            CategoriesSiesaRepository categoriesSiesaRepository
        )
        {
            this.localRepository = categoriesLocalRepository;
            this.vtexRepository = categoriesVtexRepository;
            this.siesaRepository = categoriesSiesaRepository;
        }

        public async Task<bool> Invoke()
        {
            GetAllSiesaCategories getAllSiesaCategories = new GetAllSiesaCategories(this.siesaRepository);
            GetDeltaCategories getDeltaCategories = new GetDeltaCategories(this.localRepository);
            GetCategoryBySiesaId getCategoryBySiesaId = new GetCategoryBySiesaId(this.localRepository);
            CreateCategory createCategory = new CreateCategory(this.localRepository);
            CreateVtexCategory createVtexCategory = new CreateVtexCategory(this.vtexRepository);
            UpdateCategory updateCategory = new UpdateCategory(this.localRepository);
            UpdateCategories updateCategories = new UpdateCategories(this.localRepository);
            UpdateVtexCategory updateVtexCategory = new UpdateVtexCategory(this.vtexRepository);

            Category[] siesaCategories = await getAllSiesaCategories.Invoke();

            //inactive delta categories

            Category[] deltaCategories = await getDeltaCategories.Invoke(siesaCategories);
            if(deltaCategories.Length > 0)
            {
                foreach(Category category in deltaCategories)
                {
                    category.isActive = false;
                    try
                    {
                        await updateVtexCategory.Invoke(category);
                    }
                    catch
                    {
                        category.isActive = true;
                    } 
                }
                await updateCategories.Invoke(deltaCategories);
            }

            foreach(Category siesaCategory in siesaCategories)
            {
                Category localCategory = await getCategoryBySiesaId.Invoke(siesaCategory.siesa_id);

                if(localCategory != null)
                {
                    if (localCategory.isActive)
                    {
                        // todo, ok!
                    }

                    if (!localCategory.isActive)
                    {
                        // enviar correo, categoría inactiva
                    }
                }
                else
                {
                    try
                    {
                        localCategory = await createCategory.Invoke(siesaCategory);
                        Category vtexCategory = await createVtexCategory.Invoke(localCategory);
                        localCategory.vtex_id = vtexCategory.vtex_id;
                        localCategory = await updateCategory.Invoke(localCategory);
                    }
                    catch (Exception exception)
                    {
                        //enviar correo, no fue posible crear
                    }
                }
                
            }
            return true;
        }
    }
}
