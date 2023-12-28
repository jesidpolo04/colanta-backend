using System.Threading.Tasks;
using colanta_backend.App.Prices.Domain;
using colanta_backend.App.Promotions;
using colanta_backend.App.Promotions.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace colanta_backend.App.PriceTables{
    public class PromotionalPricesRenderer{
        private PricesRepository _PricesRepository;
        private PromotionPriceCalculator _PromotionPriceCalculator;
        private PriceTablesRepository _PricesTableRepository;
        private PriceTablesVtexService _PriceTableVtexService;
        public PromotionalPricesRenderer(
            PricesRepository pricesRepository, 
            PromotionPriceCalculator promotionPriceCalculator,
            PriceTablesRepository priceTablesRepository,
            PriceTablesVtexService priceTablesVtexService
        ){
            _PricesRepository = pricesRepository;
            _PromotionPriceCalculator = promotionPriceCalculator;
            _PricesTableRepository = priceTablesRepository;
            _PriceTableVtexService = priceTablesVtexService;
        }

        public void Render(Promotion promotion){
            var priceTableName = promotion.price_table_name;
            var table = _PricesTableRepository.GetByName(priceTableName);
            if (table == null)
            {
                var priceTable = new PriceTable
                {
                    Name = priceTableName
                };
                _PricesTableRepository.Save(priceTable);
            }
            var fixedPrices = createFixedPrices(table, promotion);
            List<Task> responses = new List<Task>();
            foreach (var fixedPrice in fixedPrices)
            {
                var response = _PriceTableVtexService.AddFixedPriceToPriceTable(fixedPrice);
                responses.Add(response);
                Thread.Sleep(50);
            }
            Task.WhenAll(responses).Wait();
        }

        List<FixedPrice> createFixedPrices(PriceTable priceTable, Promotion promotion)
        {
            var fixedPrices = new List<FixedPrice>();
            foreach (var brand in promotion.brands)
            {
                var prices = _PricesRepository.getPricesByBrand((int)brand.id);
                foreach (var price in prices)
                {
                    var value = _PromotionPriceCalculator.CalculatePrice(price, promotion.percentual_discount_value);
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        PriceTableName = promotion.price_table_name,
                        ListPrice = price.price,
                        Value = value,
                        MinQuantity = 1,
                        VtexSkuId = (int)price.sku.vtex_id
                    });
                }
            }

            foreach (var category in promotion.categories)
            {
                var prices = _PricesRepository.getPricesByCategory((int)category.id);
                foreach (var price in prices)
                {
                    var value = _PromotionPriceCalculator.CalculatePrice(price, promotion.percentual_discount_value);
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        PriceTableName = promotion.price_table_name,
                        ListPrice = price.price,
                        Value = value,
                        MinQuantity = 1,
                        VtexSkuId = (int)price.sku.vtex_id
                    });
                }

            }

            foreach (var product in promotion.products)
            {
                var prices = _PricesRepository.getPricesByProduct((int)product.id);
                foreach (var price in prices)
                {
                    var value = _PromotionPriceCalculator.CalculatePrice(price, promotion.percentual_discount_value);
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        PriceTableName = promotion.price_table_name,
                        ListPrice = price.price,
                        Value = value,
                        MinQuantity = 1,
                        VtexSkuId = (int)price.sku.vtex_id
                    });
                }
            }

            foreach (var sku in promotion.skus)
            {
                var skuIds = promotion.skus.Select(sku => (int)sku.id);
                var prices = _PricesRepository.getPricesBySkuIds(skuIds.ToArray());
                foreach (var price in prices)
                {
                    var value = _PromotionPriceCalculator.CalculatePrice(price, promotion.percentual_discount_value);
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        PriceTableName = promotion.price_table_name,
                        ListPrice = price.price,
                        Value = value,
                        MinQuantity = 1,
                        VtexSkuId = (int)price.sku.vtex_id
                    });
                }
            }
            return fixedPrices;
        }
    }
}