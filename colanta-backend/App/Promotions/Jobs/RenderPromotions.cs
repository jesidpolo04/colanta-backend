namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using Shared.Domain;
    using System.Threading.Tasks;
    public class RenderPromotions
    {
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;
        private PromotionsSiesaRepository siesaRepository;
        public RenderPromotions(
                PromotionsRepository localRepository,
                PromotionsVtexRepository vtexRepository,
                PromotionsSiesaRepository siesaRepository
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task Invoke()
        {
            Promotion[] allSiesaPromotions = await this.siesaRepository.getAllPromotions();

            foreach (Promotion siesaPromotion in allSiesaPromotions)
            {
                Promotion? localPromotion = await this.localRepository.getPromotionBySiesaId(siesaPromotion.siesa_id);
                if(localPromotion == null)
                {
                    localPromotion = await this.localRepository.savePromotion(siesaPromotion);
                    Promotion vtexPromotion = await vtexRepository.savePromotion(localPromotion);
                    localPromotion.vtex_id = vtexPromotion.vtex_id;
                    await this.localRepository.updatePromotion(localPromotion);
                }
                if(localPromotion != null)
                {

                }
            }
        }
    }
}
