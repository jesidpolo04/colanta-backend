namespace colanta_backend.App.Prices.Jobs
{
    using System.Threading.Tasks;
    using App.Prices.Domain;
    public class RenderPrices
    {
        public PricesRepository localRepository; 
        public PricesVtexRepository vtexRepository ;
        public PricesSiesaRepository siesaRepository;
        public RenderPrices(PricesRepository localRepository, PricesVtexRepository vtexRepository, PricesSiesaRepository siesaRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task<bool> Invoke()
        {
            Price[] allSiesaPrices = await this.siesaRepository.getAllPrices();

            foreach(Price siesaPrice in allSiesaPrices)
            {
                Price localPrice = await this.localRepository.getPriceBySkuConcatSiesaId(siesaPrice.sku_concat_siesa_id);

                if(localPrice == null)
                {
                    localPrice = await this.localRepository.savePrice(siesaPrice);
                    Price vtexPrice = await this.vtexRepository.getPriceByVtexId(localPrice.sku.vtex_id);
                    if(vtexPrice == null)
                    {
                        await this.vtexRepository.savePrice(localPrice);
                    }
                    if(vtexPrice != null)
                    {
                        if(vtexPrice.price != localPrice.price)
                        {
                            await this.vtexRepository.savePrice(localPrice);
                        }
                    }
                }

                if(localPrice != null)
                {
                    Price vtexPrice = await this.vtexRepository.getPriceByVtexId(localPrice.sku.vtex_id);
                    if (vtexPrice == null)
                    {
                        await this.vtexRepository.savePrice(localPrice);
                    }
                    if (vtexPrice != null)
                    {
                        if (vtexPrice.price != localPrice.price)
                        {
                            await this.vtexRepository.savePrice(localPrice);
                        }
                    }
                }
            }
            return true;
        }
    }
}
