using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using colanta_backend.App.Promotions.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace colanta_backend.App.PriceTables.Scripts{
    public class RecalculateFixedPrices{
        private readonly PriceTablesRepository _PriceTablesRepository;
        private readonly PromotionsRepository _PromotionsRepository;
        private readonly PriceTableRenderer _PriceTableRenderer;
        private readonly PriceTablesVtexService _VtexService;

        public RecalculateFixedPrices(
            PriceTablesRepository priceTablesRepository, 
            PromotionsRepository promotionsRepository,
            PriceTableRenderer priceTableRenderer,
            PriceTablesVtexService vtexService
        ){
            _PriceTablesRepository = priceTablesRepository;
            _PromotionsRepository = promotionsRepository;
            _PriceTableRenderer = priceTableRenderer;
            _VtexService = vtexService;
        }

        public void Execute(){
            var promotions = _PromotionsRepository.getClusterActivePromotions();
            var count = 1;
            foreach(var promotion in promotions){
                if(promotion.price_table_name != null && promotion.price_table_name != ""){
                    var priceTable = _PriceTablesRepository.GetByName(promotion.price_table_name);
                    priceTable.DiscountPercentage = promotion.percentual_discount_value;
                    _PriceTablesRepository.Update(priceTable);
                    var fixedPrices = this._PriceTableRenderer.CreateFixedPrices(priceTable, promotion);
                    _PriceTablesRepository.UpdateFixedPrices(fixedPrices.ToArray());
                    var tasks = new List<Task>();
                    foreach(var fixedPrice in fixedPrices){
                        var task = _VtexService.AddOrUpdateFixedPriceToPriceTable(fixedPrice);
                        tasks.Add(task);
                        Thread.Sleep(80);
                    }
                    Task.WhenAll(tasks).Wait();
                    Console.WriteLine($"Price table {count}/{promotions.Length} rendered");
                    count ++;
                }
            }
        }
    }
}