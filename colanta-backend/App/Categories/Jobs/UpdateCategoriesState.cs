namespace colanta_backend.App.Categories.Jobs
{
    using Categories.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System;
    using System.Threading.Tasks;
    public class UpdateCategoriesState
    {
        private ICategoriesRepository localRepository;
        private ICategoriesVtexRepository vtexRepository;
        private ILogger logger;
        private CustomConsole console = new CustomConsole();
        public UpdateCategoriesState(ICategoriesRepository localRepository, ICategoriesVtexRepository vtexRepository, ILogger logger)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logger = logger;
        }

        public async Task Invoke()
        {
            try
            {
                Category[] localNotNullVtexCategories = await this.localRepository.GetVtexCategories();
                foreach (Category localNotNullVtexCategory in localNotNullVtexCategories)
                {
                    try
                    {
                        Category vtexCategory = await this.vtexRepository.GetCategoryByVtexId((int)localNotNullVtexCategory.VtexId);
                        if(vtexCategory.IsActive != localNotNullVtexCategory.IsActive)
                        {
                            localNotNullVtexCategory.IsActive = vtexCategory.IsActive;
                            await this.localRepository.UpdateCategory(localNotNullVtexCategory);
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
