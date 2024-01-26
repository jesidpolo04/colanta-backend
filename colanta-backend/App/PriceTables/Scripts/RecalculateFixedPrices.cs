using System;
using colanta_backend.App.Promotions.Domain;

namespace colanta_backend.App.PriceTables.Scripts{
    public class RecalculateFixedPrices{
        private readonly PriceTablesRepository _PriceTablesRepository;
        private readonly PromotionsRepository _PromotionsRepository;
        private readonly PriceTableRenderer _PriceTableRenderer;
        public RecalculateFixedPrices(
            PriceTablesRepository priceTablesRepository, 
            PromotionsRepository promotionsRepository,
            PriceTableRenderer priceTableRenderer
        ){
            _PriceTablesRepository = priceTablesRepository;
            _PromotionsRepository = promotionsRepository;
            _PriceTableRenderer = priceTableRenderer;
        }

        public void Execute(){
            var promotions = _PromotionsRepository.getActivePromotions();
            foreach(var promotion in promotions){
                if(promotion.price_table_name != null && promotion.price_table_name != ""){
                    var priceTable = _PriceTablesRepository.GetByName(promotion.price_table_name);
                    priceTable.DiscountPercentage = promotion.percentual_discount_value;
                    _PriceTablesRepository.Update(priceTable);
                    var fixedPrices = this._PriceTableRenderer.CreateFixedPrices(priceTable, promotion);
                    _PriceTablesRepository.SaveFixedPrices(fixedPrices.ToArray());
                }
            }
        }
    }
}