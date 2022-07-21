namespace colanta_backend.App.GiftCards.Application
{
    using System.Collections.Generic;
    public class ListAllGiftCardsRequestDto
    {
        public ListAllGiftCardsRequestClientDto client { get; set; }
        public ListAllGiftCardsRequestCartDto cart { get; set; }
    }

    public class ListAllGiftCardsRequestCartDto
    {
        public int grandTotal { get; set; }
        public object relationName { get; set; }
        public string redemptionCode { get; set; }
        public int discounts { get; set; }
        public int shipping { get; set; }
        public int taxes { get; set; }
        public List<ListAllGiftCardsRequestItemDto> items { get; set; }
        public int itemsTotal { get; set; }
    }

    public class ListAllGiftCardsRequestClientDto
    {
        public string id { get; set; }
        public string email { get; set; }
        public string document { get; set; }
    }

    public class ListAllGiftCardsRequestItemDto
    {
        public string productId { get; set; }
        public string id { get; set; }
        public string refId { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
