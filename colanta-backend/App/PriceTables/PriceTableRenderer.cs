using System.Threading.Tasks;
using colanta_backend.App.Prices.Domain;
using colanta_backend.App.Promotions;
using colanta_backend.App.Promotions.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace colanta_backend.App.PriceTables
{
    public class PriceTableRenderer
    {
        private readonly PricesRepository _PricesRepository;
        private readonly PromotionPriceCalculator _PromotionPriceCalculator;
        private readonly PriceTablesRepository _PricesTableRepository;
        private readonly PriceTablesVtexService _PriceTableVtexService;
        public PriceTableRenderer(
            PricesRepository pricesRepository,
            PromotionPriceCalculator promotionPriceCalculator,
            PriceTablesRepository priceTablesRepository,
            PriceTablesVtexService priceTablesVtexService
        )
        {
            _PricesRepository = pricesRepository;
            _PromotionPriceCalculator = promotionPriceCalculator;
            _PricesTableRepository = priceTablesRepository;
            _PriceTableVtexService = priceTablesVtexService;
        }

        /// <summary>
        ///     Renderiza en VTEX y en middleware la tabla de precios de una promoción.
        ///</summary> 
        public void RenderPromotionalTable(Promotion promotion)
        {
            var priceTableName = promotion.price_table_name;
            var discountPercentage = promotion.percentual_discount_value;
            var table = _PricesTableRepository.GetByName(priceTableName);
            if (table == null)
            {
                var priceTable = new PriceTable
                {
                    Name = priceTableName,
                    DiscountPercentage = discountPercentage
                };
                _PricesTableRepository.Save(priceTable);
            }
            var fixedPrices = CreateFixedPrices(table, promotion);
            List<Task> responses = new List<Task>();
            foreach (var fixedPrice in fixedPrices)
            {
                var response = _PriceTableVtexService.AddOrUpdateFixedPriceToPriceTable(fixedPrice);
                responses.Add(response);
                Thread.Sleep(50);
            }
            _PricesTableRepository.SaveFixedPrices(fixedPrices.ToArray());
            Task.WhenAll(responses).Wait();
        }

        /// <summary>
        ///     Actualiza en VTEX y en el middleware todos los precios promocionales (existentes) de un SKU
        ///</summary> 
        public void UpdateAllFixedPricesOfAnSku(Price price)
        {
            if (!price.sku.vtex_id.HasValue)
            {
                throw new System.Exception($"The sku with siesa id: {price.sku.siesa_id} doesn't have a vtex id associated");
            }
            var skuVtexId = price.sku.vtex_id.Value;
            var fixedPrices = _PricesTableRepository.GetFixedPricesBySku(skuVtexId);
            foreach (var fixedPrice in fixedPrices)
            {
                // sacar el porcentaje de la tabla decimal discountPercentage;
                var discountPercentage = fixedPrice.PriceTable.DiscountPercentage.HasValue ?
                    fixedPrice.PriceTable.DiscountPercentage.Value : 0;
                var newFixedPrice = _PromotionPriceCalculator.CalculatePrice(price, discountPercentage);
                fixedPrice.Value = newFixedPrice;
                fixedPrice.ListPrice = price.price;
                _PriceTableVtexService.AddOrUpdateFixedPriceToPriceTable(fixedPrice);
                Thread.Sleep(80);
            }
            _PricesTableRepository.UpdateFixedPrices(fixedPrices);
        }

        /// <summary>
        ///     Crea los precios fijos para la tabla de precios de una promoción.
        ///</summary> 
        public List<FixedPrice> CreateFixedPrices(PriceTable priceTable, Promotion promotion)
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

        /// <summary>
        ///     Crea y carga en VTEX un precio fijo a una tabla de precios específica.
        ///</summary> 
        public Task CreateAndUpFixedPrice(Price price, Promotion promotion)
        {
            return Task.Run(() =>
            {
                var value = _PromotionPriceCalculator.CalculatePrice(price, promotion.percentual_discount_value);
                var priceTable = _PricesTableRepository.GetByName(promotion.price_table_name);

                var fixedPrice = new FixedPrice
                {
                    PriceTable = priceTable,
                    PriceTableName = promotion.price_table_name,
                    ListPrice = price.price,
                    Value = value,
                    MinQuantity = 1,
                    VtexSkuId = (int)price.sku.vtex_id
                };
                var response = _PriceTableVtexService.AddOrUpdateFixedPriceToPriceTable(fixedPrice).Result;
            });
        }
    }
}