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
        private ILogger logger;

        public UpToVtexNullPromotions(
            PromotionsRepository localRepository,
            PromotionsVtexRepository vtexRepository,
            ILogger logger
            )
            
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logger = logger;
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
                        await this.logger.writelog(vtexException);
                    }
                }
            }
            catch (Exception exception)
            {
                await this.logger.writelog(exception);
            }
        }
    }
}
