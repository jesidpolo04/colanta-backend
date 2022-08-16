namespace colanta_backend.App.Promotions.Infraestructure
{
    using System.Collections.Generic;
    using System.Text.Json;
    using App.Promotions.Domain;
    public class ResponseCreateVtexPromotionDto
    {
        public string? idCalculatorConfiguration { get; set; }
        public string? name { get; set; }
        public string? beginDateUtc { get; set; }
        public string? endDateUtc { get; set; }
        public string? description { get; set; }
        public string? lastModified { get; set; }
        public int? daysAgoOfPurchases { get; set; }
        public bool isActive { get; set; }
        public bool isArchived { get; set; }
        public bool isFeatured { get; set; }
        public bool disableDeal { get; set; }
        public string[] activeDaysOfWeek { get; set; }

        public int? offset { get; set; }
        public bool activateGiftsMultiplier { get; set; }
        public int? newOffset { get; set; }
        public string? effecType { get; set; }
        public string? discountType { get; set; }
        public string? discountExpression { get; set; }
        public object[] maxPricesPerItems { get; set; }
        public bool cumulative { get; set; }
        public decimal nominalShippingDiscountValue { get; set; }
        public decimal absoluteShippingDiscountValue { get; set; }
        public decimal nominalDiscountValue { get; set; }
        public decimal? maximumUnitPriceDiscount { get; set; }
        public decimal percentualDiscountValue { get; set; }
        public decimal rebatePercentualDiscountValue { get; set; }
        public decimal percentualShippingDiscountValue { get; set; }
        public decimal percentualTax { get; set; }
        public decimal? shippingPercentualTax { get; set; }
        public decimal percentualDiscountValueList1 { get; set; }
        public decimal percentualDiscountValueList2 { get; set; }
        public VtexPromotionGifts skusGift { get; set; }
        public decimal? nominalRewardValue { get; set; }
        public decimal? percentualRewardValue { get; set; }
        public string? orderStatusRewardValue { get; set; }
        public int maxNumberOfAffectedItems { get; set; }
        public string? maxNumberOfAffectedItemsGroupKey { get; set; }
        public bool applyToAllShippings { get; set; }
        public decimal? nominalTax { get; set; }
        public string? origin { get; set; }
        public bool idSellerIsInclusive { get; set; }
        public object[] idsSalesChannel { get; set; }
        public bool areSalesChannelIdsExclusive { get; set; }
        public object[] marketingTags { get; set; }
        public bool marketingTagsAreNotInclusive { get; set; }
        public object[] paymentsMethods { get; set; }
        public object[] stores { get; set; }
        public object[] campaigns { get; set; }
        public bool storesAreInclusive { get; set; }
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
        public int minimumQuantityBuyTogether { get; set; }
        public int quantityToAffectBuyTogether { get; set; }
        public bool enableBuyTogetherPerSku { get; set; }
        public VtexPromotionSku[] listSku1BuyTogether { get; set; }
        public VtexPromotionSku[] listSku2BuyTogether { get; set; }
        public object[] coupon { get; set; }
        public decimal totalValueFloor { get; set; }
        public decimal totalValueCeling { get; set; }
        public bool totalValueIncludeAllItems { get; set; }
        public string? totalValueMode { get; set; }
        public object[] collections { get; set; }
        public bool collectionsIsInclusive { get; set; }
        public object[] restrictionsBins { get; set; }
        public object[] cardIssuers { get; set; }
        public decimal? totalValuePurchase { get; set; }
        public object[] slasIds { get; set; }
        public bool isSlaSelected { get; set; }
        public bool isFirstBuy { get; set; }
        public bool firstBuyIsProfileOptimistic { get; set; }
        public bool compareListPriceAndPrice { get; set; }
        public bool isDifferentListPriceAndPrice { get; set; }
        public object[] zipCodeRanges { get; set; }
        public decimal? itemMaxPrice { get; set; }
        public decimal? itemMinPrice { get; set; }
        public int? installment { get; set; }
        public bool isMinMaxInstallments { get; set; }
        public int? minInstallment { get; set; }
        public int? maxInstallment { get; set; }
        public object[] merchants { get; set; }
        public string[] clusterExpressions { get; set; }
        public string? clusterOperator { get; set; }
        public object[] paymentsRules { get; set; }
        public object[] giftListTypes { get; set; }
        public object[] productsSpecifications { get; set; }
        public object[] affiliates { get; set; }
        public int? maxUsage { get; set; }
        public int? maxUsagePerClient { get; set; }
        public bool shouldDistributeDiscountAmongMatchedItems { get; set; }
        public bool multipleUsePerClient { get; set; }
        public string type { get; set; }
        public bool useNewProgressiveAlgorithm { get; set; }
        public object[] percentualDiscountValueList { get; set; }

        public Promotion getPromotionFromDto()
        {
            Promotion promotion = new Promotion();

            promotion.vtex_id = this.idCalculatorConfiguration;
            promotion.type = this.type;
            promotion.name = this.name;
            promotion.begin_date_utc = this.beginDateUtc;
            promotion.end_date_utc = this.endDateUtc;
            promotion.is_active = this.isActive;
            promotion.nominal_discount_value = this.nominalDiscountValue;
            promotion.percentual_discount_value = this.percentualDiscountValue;
            promotion.percentual_shipping_discount_value = this.percentualShippingDiscountValue;
            promotion.max_number_of_affected_items = this.maxNumberOfAffectedItems;
            promotion.max_number_of_affected_items_group_key = this.maxNumberOfAffectedItemsGroupKey;
            promotion.minimum_quantity_buy_together = this.minimumQuantityBuyTogether;
            promotion.quantity_to_affect_buy_together = this.quantityToAffectBuyTogether;
            promotion.percentual_discount_value = this.percentualDiscountValue;
            promotion.percentual_discount_value_list_1 = this.percentualDiscountValueList1;
            promotion.percentual_discount_value_list_2 = this.percentualDiscountValueList2;
            promotion.total_value_floor = this.totalValueFloor;
            promotion.total_value_celing = this.totalValueCeling;
            promotion.cumulative = this.cumulative;
            promotion.multiple_use_per_client = this.multipleUsePerClient;

            return promotion;
        }
    }
}
