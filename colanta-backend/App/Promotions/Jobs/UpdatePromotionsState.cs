namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using System;
    using System.Threading.Tasks;
    public class UpdatePromotionsState
    {
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;

        public UpdatePromotionsState(PromotionsRepository localRepository, PromotionsVtexRepository vtexRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
        }

        public async Task Invoke()
        {
            PromotionSummary[] promotionsSummaries = await this.vtexRepository.getPromotionsList();
            foreach(PromotionSummary promotionSummary in promotionsSummaries)
            {
                try
                {
                    Promotion localPromotion = await this.localRepository.getPromotionByVtexId(promotionSummary.vtexId);
                    if (localPromotion == null) continue;
                    if(localPromotion.is_active != promotionSummary.isActive)
                    {
                        localPromotion.is_active = promotionSummary.isActive;
                        await this.localRepository.updatePromotion(localPromotion);
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);
                }
            }
        }
    }
}
