﻿namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    public class VtexOrderDto
    {
        public string emailTracked { get; set; }
        public string? approvedBy { get; set; }
        public string? cancelledBy { get; set; }
        public string? cancelReason { get; set; }
        public string orderId { get; set; }
        public string sequence { get; set; }
        public string marketplaceOrderId { get; set; }
        public string marketplaceServicesEndpoint { get; set; }
        public string sellerOrderId { get; set; }
        public string origin { get; set; }
        public string affiliateId { get; set; }
        public string salesChannel { get; set; }
        public string? merchantName { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public decimal value { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastChange { get; set; }
        public string? orderGroup { get; set; }
        public List<Total> totals { get; set; }
        public List<Item> items { get; set; }
        public List<object> marketplaceItems { get; set; }
        public ClientProfileData clientProfileData { get; set; }
        public object giftRegistryData { get; set; }
        public object marketingData { get; set; }
        public RatesAndBenefitsData ratesAndBenefitsData { get; set; }
        public ShippingData shippingData { get; set; }
        public PaymentData paymentData { get; set; }
        public PackageAttachment packageAttachment { get; set; }
        public List<Seller> sellers { get; set; }
        public object callCenterOperatorData { get; set; }
        public string followUpEmail { get; set; }
        public object lastMessage { get; set; }
        public string hostname { get; set; }
        public object invoiceData { get; set; }
        public ChangesAttachment changesAttachment { get; set; }
        public OpenTextField? openTextField { get; set; }
        public int roundingError { get; set; }
        public string orderFormId { get; set; }
        public object commercialConditionData { get; set; }
        public bool isCompleted { get; set; }
        public object customData { get; set; }
        public StorePreferencesData storePreferencesData { get; set; }
        public bool allowCancellation { get; set; }
        public bool allowEdition { get; set; }
        public bool isCheckedIn { get; set; }
        public Marketplace marketplace { get; set; }
        public DateTime? authorizedDate { get; set; }
        public object invoicedDate { get; set; }
    }

    public class AdditionalInfo
    {
        public string brandName { get; set; }
        public string brandId { get; set; }
        public string categoriesIds { get; set; }
        public string productClusterId { get; set; }
        public string commercialConditionId { get; set; }
        public Dimension dimension { get; set; }
        public string? offeringInfo { get; set; }
        public string? offeringType { get; set; }
        public string? offeringTypeId { get; set; }
    }

    public class Address
    {
        public string addressType { get; set; }
        public string receiverName { get; set; }
        public string addressId { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string neighborhood { get; set; }
        public string complement { get; set; }
        public object reference { get; set; }
        public List<object> geoCoordinates { get; set; }
    }

    public class BillingAddress
    {
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string neighborhood { get; set; }
        public string complement { get; set; }
        public string reference { get; set; }
        public List<decimal> geoCoordinates { get; set; }
    }

    public class ChangesAttachment
    {
        public string id { get; set; }
        public List<ChangesDatum> changesData { get; set; }
    }

    public class ChangesDatum
    {
        public string reason { get; set; }
        public int discountValue { get; set; }
        public int incrementValue { get; set; }
        public List<object> itemsAdded { get; set; }
        public List<ItemsRemoved> itemsRemoved { get; set; }
        public Receipt receipt { get; set; }
    }

    public class ClientProfileData
    {
        public string id { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string documentType { get; set; }
        public string document { get; set; }
        public string phone { get; set; }
        public string corporateName { get; set; }
        public string tradeName { get; set; }
        public string corporateDocument { get; set; }
        public string stateInscription { get; set; }
        public string corporatePhone { get; set; }
        public bool isCorporate { get; set; }
        public string userProfileId { get; set; }
        public object customerClass { get; set; }
    }

    public class ConnectorResponses
    {
    }

    public class Content
    {
    }

    public class CurrencyFormatInfo
    {
        public int CurrencyDecimalDigits { get; set; }
        public string CurrencyDecimalSeparator { get; set; }
        public string CurrencyGroupSeparator { get; set; }
        public int CurrencyGroupSize { get; set; }
        public bool StartsWithCurrencySymbol { get; set; }
    }

    public class DeliveryId
    {
        public string courierId { get; set; }
        public string courierName { get; set; }
        public string? dockId { get; set; }
        public int quantity { get; set; }
        public string warehouseId { get; set; }
    }

    public class Dimension
    {
        public decimal cubicweight { get; set; }
        public decimal height { get; set; }
        public decimal length { get; set; }
        public decimal weight { get; set; }
        public decimal width { get; set; }
    }

    public class Item
    {
        public string uniqueId { get; set; }
        public string id { get; set; }
        public string productId { get; set; }
        public string? ean { get; set; }
        public string lockId { get; set; }
        public ItemAttachment itemAttachment { get; set; }
        public List<object> attachments { get; set; }
        public int quantity { get; set; }
        public string seller { get; set; }
        public string name { get; set; }
        public string refId { get; set; }
        public decimal price { get; set; }
        public decimal listPrice { get; set; }
        public decimal? manualPrice { get; set; }
        public List<PriceTag> priceTags { get; set; }
        public string imageUrl { get; set; }
        public string detailUrl { get; set; }
        public List<object> components { get; set; }
        public List<object> bundleItems { get; set; }
        public List<object> @params { get; set; }
        public List<object> offerings { get; set; }
        public string sellerSku { get; set; }
        public string? priceValidUntil { get; set; }
        public decimal commission { get; set; }
        public decimal tax { get; set; }
        public string? preSaleDate { get; set; }
        public AdditionalInfo additionalInfo { get; set; }
        public string measurementUnit { get; set; }
        public decimal? unitMultiplier { get; set; }
        public decimal sellingPrice { get; set; }
        public bool isGift { get; set; }
        public string? shippingPrice { get; set; }
        public decimal? rewardValue { get; set; }
        public decimal? freightCommission { get; set; }
        public PriceDefinition priceDefinition { get; set; }
        public string? taxCode { get; set; }
        //public object? parentItemIndex { get; set; }
        //public object? parentAssemblyBinding { get; set; }
    }

    public class ItemAttachment
    {
        public Content content { get; set; }
        public string? name { get; set; }
    }

    public class ItemsRemoved
    {
        public string id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public decimal? unitMultiplier { get; set; }
    }

    public class LogisticsInfo
    {
        public int itemIndex { get; set; }
        public string selectedSla { get; set; }
        public string? lockTTL { get; set; }
        public decimal price { get; set; }
        public decimal? listPrice { get; set; }
        public decimal sellingPrice { get; set; }
        public object deliveryWindow { get; set; }
        public string deliveryCompany { get; set; }
        public string shippingEstimate { get; set; }
        public DateTime? shippingEstimateDate { get; set; }
        public List<Sla> slas { get; set; }
        public List<string> shipsTo { get; set; }
        public List<DeliveryId> deliveryIds { get; set; }
        public string deliveryChannel { get; set; }
        public PickupStoreInfo pickupStoreInfo { get; set; }
        public string addressId { get; set; }
        public string polygonName { get; set; }
    }

    public class Marketplace
    {
        public string baseURL { get; set; }
        public object? isCertified { get; set; }
        public string name { get; set; }
    }

    public class PackageAttachment
    {
        public List<object> packages { get; set; }
    }

    public class Payment
    {
        public string id { get; set; }
        public string paymentSystem { get; set; }
        public string paymentSystemName { get; set; }
        public decimal value { get; set; }
        public int installments { get; set; }
        public decimal referenceValue { get; set; }
        public object cardHolder { get; set; }
        public object firstDigits { get; set; }
        public object lastDigits { get; set; }
        public string url { get; set; }
        public string giftCardId { get; set; }
        public string giftCardName { get; set; }
        public string giftCardCaption { get; set; }
        public string giftCardProvider { get; set; }
        public string redemptionCode { get; set; }
        public string group { get; set; }
        public string tid { get; set; }
        public string dueDate { get; set; }
        public ConnectorResponses connectorResponses { get; set; }
        public BillingAddress billingAddress { get; set; }
    }

    public class PaymentData
    {
        public List<Transaction> transactions { get; set; }
    }

    public class PickupStoreInfo
    {
        public string additionalInfo { get; set; }
        public Address address { get; set; }
        public string dockId { get; set; }
        public string friendlyName { get; set; }
        public bool isPickupStore { get; set; }
    }

    public class PriceDefinition
    {
        public List<SellingPrice> sellingPrice { get; set; }
        public decimal total { get; set; }
    }

    public class RatesAndBenefitsData
    {
        public string id { get; set; }
        public List<object> rateAndBenefitsIdentifiers { get; set; }
    }

    public class Receipt
    {
        public DateTime? date { get; set; }
        public string orderId { get; set; }
        public string receipt { get; set; }
    }

    public class SelectedAddress
    {
        public string addressId { get; set; }
        public string addressType { get; set; }
        public string receiverName { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string reference { get; set; }
        public List<object> geoCoordinates { get; set; }
    }

    public class Seller
    {
        public string id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
    }

    public class SellingPrice
    {
        public int quantity { get; set; }
        public decimal value { get; set; }
    }

    public class ShippingData
    {
        public string id { get; set; }
        public Address address { get; set; }
        public List<LogisticsInfo> logisticsInfo { get; set; }
        public object trackingHints { get; set; }
        public List<SelectedAddress> selectedAddresses { get; set; }
    }

    public class Sla
    {
        public string id { get; set; }
        public string name { get; set; }
        public string shippingEstimate { get; set; }
        public object deliveryWindow { get; set; }
        public decimal price { get; set; }
        public string deliveryChannel { get; set; }
        public PickupStoreInfo pickupStoreInfo { get; set; }
        public string polygonName { get; set; }
    }

    public class StorePreferencesData
    {
        public string countryCode { get; set; }
        public string currencyCode { get; set; }
        public CurrencyFormatInfo currencyFormatInfo { get; set; }
        public int currencyLocale { get; set; }
        public string currencySymbol { get; set; }
        public string timeZone { get; set; }
    }

    public class Total
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal value { get; set; }
    }

    public class Transaction
    {
        public bool isActive { get; set; }
        public string transactionId { get; set; }
        public string merchantName { get; set; }
        public List<Payment> payments { get; set; }
    }

    public class PriceTag
    {
        public string name { get; set; }
        public decimal value { get; set; }
        public decimal rawValue { get; set; }
        public string identifier { get; set; }
        public bool isPercentual { get; set; }
    }

    public class OpenTextField
    {
        public string value { get; set; }
    }
}
