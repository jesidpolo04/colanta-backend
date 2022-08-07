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
            Promotion[] promotions = await this.localRepository.getVtexPromotions();
            foreach(Promotion promotion in promotions)
            {
                try
                {
                    Promotion vtexPromotion = await this.vtexRepository.getPromotionByVtexId(promotion.vtex_id.ToString(), promotion.business);
                    if(promotion.is_active != vtexPromotion.is_active)
                    {
                        promotion.is_active = vtexPromotion.is_active;
                        await this.localRepository.updatePromotion(promotion);
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
