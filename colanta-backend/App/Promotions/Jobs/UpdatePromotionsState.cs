namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using System;
    using System.Threading.Tasks;
    using Shared.Domain;
    using Shared.Application;
    public class UpdatePromotionsState
    {
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;
        private ILogger logger;
        private CustomConsole console = new CustomConsole();

        public UpdatePromotionsState(PromotionsRepository localRepository, PromotionsVtexRepository vtexRepository, ILogger logger)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logger = logger;
        }

        public async Task Invoke()
        {
            try
            {
                PromotionSummary[] promotionsSummaries = await this.vtexRepository.getPromotionsList();
                foreach (PromotionSummary promotionSummary in promotionsSummaries)
                {
                    try
                    {
                        Promotion localPromotion = await this.localRepository.getPromotionByVtexId(promotionSummary.vtexId);
                        if (localPromotion == null) continue;
                        if (localPromotion.is_active != promotionSummary.isActive)
                        {
                            localPromotion.is_active = promotionSummary.isActive;
                            await this.localRepository.updatePromotion(localPromotion);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        Console.WriteLine(exception.StackTrace);
                    }
                }
            }
            catch (Exception exception)
            {
                await logger.writelog(exception);
                console.throwException(exception.Message);
            }
        }
    }
}
