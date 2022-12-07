namespace colanta_backend.App.Prices.Jobs
{
    using System.Threading.Tasks;
    using System.Linq;
    using System;
    using Products.Domain;
    using Prices.Domain;
    using Shared.Domain;

    public class NotifyMissingPrices : IDisposable
    {
        private SkusRepository skusRepository;
        private ProductsSiesaRepository productsSiesaRepository;
        private PricesSiesaRepository priceSiesaRepository;
        private ILogger logger;
        private INotifyMissingPriceMail mail;

        public NotifyMissingPrices(SkusRepository skusRepository, ProductsSiesaRepository productsSiesaRepository, PricesSiesaRepository priceSiesaRepository, ILogger logger, INotifyMissingPriceMail mail)
        {
            this.skusRepository = skusRepository;
            this.productsSiesaRepository = productsSiesaRepository;
            this.priceSiesaRepository = priceSiesaRepository;
            this.logger = logger;
            this.mail = mail;
        }

        public void Dispose()
        {
        }

        public async Task Invoke()
        {
            var activeSkus = this.skusRepository.getVtexSkus().Result.Where(sku => sku.is_active == true);
            var siesaPrices = this.priceSiesaRepository.getAllPrices().Result;
            foreach(var sku in activeSkus)
            {
                try
                {
                    var prices = siesaPrices.Where(price => price.sku_concat_siesa_id == sku.concat_siesa_id);
                    if (prices.ToArray().Length == 0)
                    {
                        this.mail.sendMail(sku);
                    }
                }
                catch(Exception exception)
                {
                    await this.logger.writelog(exception);
                }
            }
        }
    }
}
