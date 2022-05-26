namespace colanta_backend.App.GiftCards.Controllers
{
    using GiftCards.Domain;
    public class GiftCardProviderDto
    {
        public string id { get; set; }
        public string provider = "siesa";
        public decimal balance { get; set; }
        public decimal? total_balance { get; set; }
        public string relationName { get; set; }
        public string? caption { get; set; }
        public string? groupName { get; set; }

        public void setDtoFromGiftCard(GiftCard giftCard)
        {
            this.id = giftCard.siesa_id;
            this.balance = giftCard.balance;
            this.relationName = giftCard.name;
            this.caption = "";
            this.groupName = "";
        }
    }
}
