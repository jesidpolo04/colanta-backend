using System;
using System.Threading.Tasks;
using colanta_backend.App.Promotions.Domain;
using Microsoft.Extensions.Logging;

namespace colanta_backend.App.PriceTables.Scripts{
    public class RenderPriceTable{
        private readonly PriceTablesVtexService _Service;
        private readonly PriceTableRenderer _Renderer;
        private readonly PriceTablesRepository _Repository;
        private readonly ILogger<RenderPriceTable> _Logger;
        private readonly PromotionsRepository _PromotionsRepository;

        public RenderPriceTable(
            PriceTablesVtexService service, 
            PriceTablesRepository repository, 
            PriceTableRenderer renderer, 
            PromotionsRepository promotionsRepository, 
            ILogger<RenderPriceTable> logger)
        {
            _Service = service;
            _Renderer = renderer;
            _Repository = repository;
            _Logger = logger;
            _PromotionsRepository = promotionsRepository;
        }

        public async Task Execute(string promotionSiesaId)
        {
            try{
                Promotion promotion = await _PromotionsRepository.getPromotionBySiesaId(promotionSiesaId);
                _Logger.LogInformation("Starting to render fixed prices for promotion: {PromotionName}", promotion.siesa_id);
                PriceTable priceTable = _Repository.GetByName(promotion.price_table_name);
                var prices = _Renderer.CreateFixedPrices(priceTable, promotion);
                _Logger.LogDebug("Prices to be rendered: {Prices}", prices);
                foreach (var price in prices)
                {
                    try{
                        _Logger.LogDebug("Rendering price for sku with vtex id: {VtexId}", price.VtexSkuId);
                        _ = _Service.AddOrUpdateFixedPriceToPriceTable(price);
                    }catch(Exception e){
                        _Logger.LogError(e, "Error rendering price for sku with vtex id: {VtexId}", price.VtexSkuId);
                    }
                }
                _Logger.LogDebug("Saving fixed prices for promotion: {PromotionName} in database", promotion.siesa_id);
                _Repository.SaveFixedPrices(prices.ToArray());
            }catch(Exception e){
                _Logger.LogError(e, "Error executing RenderPriceTable for promotion: {PromotionName}", promotionSiesaId);
            }
        }
    }
}