using System;
using System.Threading.Tasks;
using colanta_backend.App.PriceTables;
using colanta_backend.App.Promotions.Domain;
using Microsoft.Extensions.Logging;

namespace colanta_backend.App.Promotions.Jobs{
    public class CleanFixedPricesFromExpiredPromotion{
        private readonly PromotionsRepository _promotionsRepository;
        private readonly PriceTablesRepository _priceTablesRepository;
        private readonly PriceTablesVtexService _priceTablesVtexService;
        private readonly ILogger<CleanFixedPricesFromExpiredPromotion> _logger;

        public CleanFixedPricesFromExpiredPromotion(
            PromotionsRepository promotionsRepository,
            PriceTablesRepository priceTablesRepository,
            PriceTablesVtexService priceTablesVtexService,
            ILogger<CleanFixedPricesFromExpiredPromotion> logger
        ){
            _promotionsRepository = promotionsRepository;
            _priceTablesRepository = priceTablesRepository;
            _priceTablesVtexService = priceTablesVtexService;
            _logger = logger;
        }

        public async Task Execute(){
            _logger.LogInformation("Cleaning fixed prices from expired promotions");
            Promotion[] expiredPromotions = await _promotionsRepository.GetExpiredPromotions();
            _logger.LogDebug("Found {PromotionsCount} expired promotions", expiredPromotions.Length);
            foreach(Promotion promotion in expiredPromotions){
                _logger.LogInformation("Cleaning fixed prices from expired promotion {PromotionSiesaId}", promotion.siesa_id);
                string priceTableName = promotion.price_table_name;
                FixedPrice[] fixedPrices = _priceTablesRepository.GetFixedPricesByPriceTableName(priceTableName);
                _logger.LogDebug("Found {FixedPricesCount} fixed prices in local price table {PriceTableName}", fixedPrices.Length, priceTableName);
                foreach(var fixedPrice in fixedPrices){
                    try{
                        _logger.LogInformation("Deleting fixed price {FixedPriceSkuId} from price table {PriceTableName}", fixedPrice.VtexSkuId, priceTableName);
                        await _priceTablesVtexService.DeleteFixedPriceFromPriceTable(priceTableName, fixedPrice.VtexSkuId);
                        _priceTablesRepository.DeleteFixedPrice(fixedPrice);
                    }catch(Exception exception){
                        _logger.LogError(exception, "Error cleaning fixed price {FixedPriceSkuId} from price table {PriceTableName}", fixedPrice.VtexSkuId, priceTableName);
                    }
                }
            }
        }

    }
}

