using colanta_backend.App.Prices.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Prices.Infraestructure
{
    using App.Shared.Infraestructure;
    using App.Prices.Domain;
    using App.Products.Domain;
    using App.Products.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    public class PricesEFRepository : Domain.PricesRepository
    {
        private ColantaContext dbContext;
        public PricesEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<Price?> getPriceBySkuConcatSiesaId(string concat_siesa_id)
        {
            var efPrices = this.dbContext.Prices.Where(e => e.sku_concat_siesa_id == concat_siesa_id);

            if (efPrices.ToArray().Length > 0)
            {
                EFPrice efPrice = efPrices.First();
                EFSku efSku = this.dbContext.Skus.Find(efPrice.sku_id);

                efPrice.sku = efSku;

                return efPrice.getPriceFromEfPrice();
            }

            return null;
        }

        public async Task<Price?> getPriceBySkuId(int sku_id)
        {
            var efPrices = this.dbContext.Prices.Where(e => e.sku.id == sku_id);

            if (efPrices.ToArray().Length > 0)
            {
                EFPrice efPrice = efPrices.First();
                EFSku efSku = this.dbContext.Skus.Find(efPrice.sku_id);

                efPrice.sku = efSku;

                return efPrice.getPriceFromEfPrice();
            }

            return null;
        }

        public async Task<Price> savePrice(Price price)
        {
            EFPrice efPrice = new EFPrice();
            efPrice.setEfPriceFromPrice(price);
            EFSku efSku = this.dbContext.Skus.Where(sku => sku.concat_siesa_id == price.sku_concat_siesa_id).First();
            efPrice.sku = efSku;
            this.dbContext.Add(efPrice);
            this.dbContext.SaveChanges();
            return await this.getPriceBySkuConcatSiesaId(price.sku_concat_siesa_id);
        }

        public async Task<Price> updatePrice(Price price)
        {
            EFPrice efPrice = this.dbContext.Prices.Find(price.id);

            efPrice.price = price.price;
            efPrice.business = price.business;
            efPrice.sku_concat_siesa_id = price.sku_concat_siesa_id;
            efPrice.sku_id = price.sku_id;
            dbContext.SaveChanges();

            return price;
        }

        public async Task<Price[]> updatePrices(Price[] prices)
        {
            foreach(Price price in prices)
            {
                EFPrice efPrice = this.dbContext.Prices.Find(price.id);

                efPrice.price = price.price;
                efPrice.business = price.business;
                efPrice.sku_concat_siesa_id = price.sku_concat_siesa_id;
                efPrice.sku_id = price.sku_id;
            }
            dbContext.SaveChanges();
            return prices;
        }
    }
}
