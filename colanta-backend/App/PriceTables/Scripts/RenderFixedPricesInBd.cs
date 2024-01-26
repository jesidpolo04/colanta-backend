using System;
using colanta_backend.App.Promotions.Domain;

namespace colanta_backend.App.PriceTables.Scripts{
    public class RenderFixedPricesInBd{
        private readonly PriceTablesRepository _PriceTablesRepository;
        private readonly PromotionsRepository _PromotionsRepository;
        private readonly PriceTableRenderer _PriceTableRenderer;
        public RenderFixedPricesInBd(
            PriceTablesRepository priceTablesRepository, 
            PromotionsRepository promotionsRepository,
            PriceTableRenderer priceTableRenderer
        ){
            _PriceTablesRepository = priceTablesRepository;
            _PromotionsRepository = promotionsRepository;
        }

        public void Execute(){
            var promotions = _PromotionsRepository.getActivePromotions();
            foreach(var promotion in promotions){
                if(promotion.price_table_name != null && promotion.price_table_name != ""){
                    var priceTable = _PriceTablesRepository.GetByName(promotion.price_table_name);
                    var fixedPrices = this._PriceTableRenderer.CreateFixedPrices(priceTable, promotion);
                    _PriceTablesRepository.SaveFixedPrices(fixedPrices.ToArray());
                }
            }
        }
    }
}