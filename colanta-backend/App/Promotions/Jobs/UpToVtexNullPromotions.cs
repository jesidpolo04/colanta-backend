namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System;
    using System.Threading.Tasks;
    public class UpToVtexNullPromotions
    {
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;

        public UpToVtexNullPromotions(
            PromotionsRepository localRepository,
            PromotionsVtexRepository vtexRepository

            )
            
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
        }

        public async Task Invoke()
        {
            try
            {
                Promotion[] nullPromotions = await this.localRepository.getVtexNullPromotions();
                foreach(Promotion nullPromotion in nullPromotions)
                {
                    try
                    {
                        Promotion vtexPromotion = await this.vtexRepository.savePromotion(nullPromotion);
                        nullPromotion.vtex_id = vtexPromotion.vtex_id;
                        await this.localRepository.updatePromotion(nullPromotion);
                    }
                    catch(VtexException vtexException)
                    {

                    }
                }
            }
            catch (Exception exception)
            {

            }
        }
    }
}
