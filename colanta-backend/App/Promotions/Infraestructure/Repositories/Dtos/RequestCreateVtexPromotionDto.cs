namespace colanta_backend.App.Promotions.Infraestructure
{
    using System.Collections.Generic;
    using System.Text.Json;
    using App.Promotions.Domain;
    public class RequestCreateVtexPromotionDto
    {
        public string? idCalculatorConfiguration { get; set; }
        public string? calculatorConfiguration { get; set; }
        public string? name { get; set; }
        public string? beginDateUtc { get; set; }
        public string? endDateUtc { get; set; }
        public bool isActive { get; set; }

        public string? discountType { get; set; }
        public bool cumulative { get; set; }
        public decimal nominalShippingDiscountValue { get; set; }
        public decimal nominalDiscountValue { get; set; }
        public decimal percentualDiscountValue { get; set; }
        public decimal percentualShippingDiscountValue { get; set; }
        public decimal percentualDiscountValueList1 { get; set; }
        public decimal percentualDiscountValueList2 { get; set; }
        public VtexPromotionGifts skusGift { get; set; }
        public int? maxNumberOfAffectedItems { get; set; }
        public string? maxNumberOfAffectedItemsGroupKey { get; set; }
        public VtexPromotionCategory[] categories { get; set; }
        public bool categoriesAreInclusive { get; set; }
        public VtexPromotionBrand[] brands { get; set; }
        public bool brandsAreInclusive { get; set; }
        public VtexPromotionProduct[] products { get; set; }
        public bool productsAreInclusive { get; set; }
        public VtexPromotionSku[] skus { get; set; }
        public bool skusAreInclusive { get; set; }
        public object[] collections1BuyTogether { get; set; }
        public object[] collections2BuyTogether { get; set; }
        public int? minimumQuantityBuyTogether { get; set; }
        public int? quantityToAffectBuyTogether { get; set; }
        public bool enableBuyTogetherPerSku { get; set; }
        public VtexPromotionSku[] listSku1BuyTogether { get; set; }
        public VtexPromotionSku[] listSku2BuyTogether { get; set; }
        public decimal? totalValueFloor { get; set; }
        public decimal? totalValueCeling { get; set; }
        public decimal? itemMaxPrice { get; set; }
        public decimal? itemMinPrice { get; set; }
        public object[] clusterExpressions { get; set; }
        public string? clusterOperator { get; set; }
        public int? maxUsage { get; set; }
        public int? maxUsagePerClient { get; set; }
        public bool multipleUsePerClient { get; set; }
        public string type { get; set; }
        public string origin { get; set; }
    }
}
