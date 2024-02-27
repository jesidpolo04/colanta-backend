using colanta_backend.App.Prices.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Prices.Infraestructure
{
    using App.Shared.Infraestructure;
    using App.Prices.Domain;
    using App.Products.Domain;
    using App.Products.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class PricesEFRepository : Domain.PricesRepository
    {
        private ColantaContext dbContext;
        public PricesEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<Price?> getPriceBySkuConcatSiesaId(string concat_siesa_id)
        {
            var efPrices = this.dbContext.Prices
                .Include(price => price.sku)
                .ThenInclude(sku => sku.product)
                .Where(e => e.sku_concat_siesa_id == concat_siesa_id);

            if (efPrices.ToArray().Length > 0)
            {
                EFPrice efPrice = efPrices.First();
                return efPrice.getPriceFromEfPrice();
            }

            return null;
        }

        public async Task<Price?> getPriceBySkuId(int sku_id)
        {
            var efPrices = this.dbContext.Prices
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .Where(e => e.sku.id == sku_id);

            if (efPrices.ToArray().Length > 0)
            {
                EFPrice efPrice = efPrices.First();
                return efPrice.getPriceFromEfPrice();
            }

            return null;
        }

        public Price[] getPricesByBrand(int brandId)
        {
            var dbPrices = dbContext.Prices
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .Where(price => price.sku.product.brand.id == brandId).ToList();
            return dbPrices.Select(dbPrice => dbPrice.getPriceFromEfPrice()).ToArray();
        }

        public Price[] getPricesByCategory(int categoryId)
        {
            var dbPrices = dbContext.Prices
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .Where(price => price.sku.product.category.id == categoryId).ToList();
            return dbPrices.Select(dbPrice => dbPrice.getPriceFromEfPrice()).ToArray();
        }

        public Price[] getPricesByProduct(int productId)
        {
            var dbPrices = dbContext.Prices
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .Where(price => price.sku.product.id == productId).ToList();
            return dbPrices.Select(dbPrice => dbPrice.getPriceFromEfPrice()).ToArray();
        }

        public Price[] getPricesBySkuIds(int[] skuIds)
        {
            var dbPrices = dbContext.Prices
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .Where(price => skuIds.Contains((int)price.sku.id)).ToList();
            return dbPrices.Select(dbPrice => dbPrice.getPriceFromEfPrice()).ToArray();
        }

        public Price? getPriceWithCategoryAndBrand(string concatSiesaId)
        {
            var dbPrices = dbContext.Prices
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .ThenInclude(product => product.brand)
            .Include(price => price.sku)
            .ThenInclude(sku => sku.product)
            .ThenInclude(product => product.category)
            .Where(price => price.sku_concat_siesa_id == concatSiesaId).ToList();
            return dbPrices.Count > 0 ? dbPrices.First().getPriceFromEfPrice() : null;
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
            efPrice.base_price = price.base_price;
            efPrice.business = price.business;
            efPrice.sku_concat_siesa_id = price.sku_concat_siesa_id;
            efPrice.sku_id = price.sku_id;
            dbContext.SaveChanges();

            return price;
        }

        public async Task<Price[]> updatePrices(Price[] prices)
        {
            foreach (Price price in prices)
            {
                EFPrice efPrice = this.dbContext.Prices.Find(price.id);

                efPrice.price = price.price;
                efPrice.base_price = price.base_price;
                efPrice.business = price.business;
                efPrice.sku_concat_siesa_id = price.sku_concat_siesa_id;
                efPrice.sku_id = price.sku_id;
            }
            dbContext.SaveChanges();
            return prices;
        }
    }
}
