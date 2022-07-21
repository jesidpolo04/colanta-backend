namespace colanta_backend.App.GiftCards.Application
{
    using System;
    using System.Collections.Generic;
    public class GetTransactionByIdResponseDto
    {
        public double value { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public string requestId { get; set; }
        public GetTransactionByIdResponseOrderInfoDto orderInfo { get; set; }
        public GetTransactionByIdResponseSettlementDto settlement { get; set; }
        public GetTransactionByIdResponseCancellationDto cancellation { get; set; }
        public GetTransactionByIdResponseAuthorizationDto authorization { get; set; }
        public string operation { get; set; }
    }

    public class GetTransactionByIdResponseAuthorizationDto
    {
        public string href { get; set; }
    }

    public class GetTransactionByIdResponseCancellationDto
    {
        public string href { get; set; }
    }

    public class GetTransactionByIdResponseCartDto
    {
        public List<GetTransactionByIdResponseItemDto> items { get; set; }
        public int grandTotal { get; set; }
        public int discounts { get; set; }
        public int shipping { get; set; }
        public int taxes { get; set; }
        public int itemsTotal { get; set; }
    }

    public class GetTransactionByIdResponseClientProfileDto
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string document { get; set; }
        public string documentType { get; set; }
        public string phone { get; set; }
        public DateTime birthDate { get; set; }
        public bool isCorporate { get; set; }
    }

    public class GetTransactionByIdResponseItemDto
    {
        public string id { get; set; }
        public string productId { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int sellingPrice { get; set; }
        public int quantity { get; set; }
        public int totalShippingDiscount { get; set; }
        public int totalDiscount { get; set; }
        public List<object> priceTags { get; set; }
    }

    public class GetTransactionByIdResponseOrderInfoDto
    {
        public string orderId { get; set; }
        public int sequence { get; set; }
        public GetTransactionByIdResponseCartDto cart { get; set; }
        public GetTransactionByIdResponseClientProfileDto clientProfile { get; set; }
        public GetTransactionByIdResponseShippingDto shipping { get; set; }
    }

    public class GetTransactionByIdResponseSettlementDto
    {
        public string href { get; set; }
    }

    public class GetTransactionByIdResponseShippingDto
    {
        public string receiverName { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string neighborhood { get; set; }
        public string complement { get; set; }
        public string reference { get; set; }
    }

}
