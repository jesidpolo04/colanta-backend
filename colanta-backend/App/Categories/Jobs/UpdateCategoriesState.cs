namespace colanta_backend.App.Categories.Jobs
{
    using Categories.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System;
    using System.Threading.Tasks;
    public class UpdateCategoriesState
    {
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private ILogger logger;
        private CustomConsole console = new CustomConsole();
        public UpdateCategoriesState(CategoriesRepository localRepository, CategoriesVtexRepository vtexRepository, ILogger logger)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logger = logger;
        }

        public async Task Invoke()
        {
            try
            {
                Category[] localNotNullVtexCategories = await this.localRepository.getVtexCategories();
                foreach (Category localNotNullVtexCategory in localNotNullVtexCategories)
                {
                    try
                    {
                        Category vtexCategory = await this.vtexRepository.getCategoryByVtexId((int)localNotNullVtexCategory.vtex_id);
                        if(vtexCategory.isActive != localNotNullVtexCategory.isActive)
                        {
                            localNotNullVtexCategory.isActive = vtexCategory.isActive;
                            await this.localRepository.updateCategory(localNotNullVtexCategory);
                        }
                    }
                    catch(VtexException vtexException)
                    {
                        console.throwException(vtexException.Message);
                        await logger.writelog(vtexException);
                    }
                }
            }
            catch (Exception exception)
            {
                console.throwException(exception.Message);
                await logger.writelog(exception);
            }
        }
    }
}
