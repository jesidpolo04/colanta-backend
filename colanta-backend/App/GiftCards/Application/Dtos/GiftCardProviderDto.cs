namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    public class GiftCardProviderDto
    {
        public string id { get; set; }
        public string provider { get; set; }
        public decimal balance { get; set; }
        public decimal? totalBalance { get; set; }
        public string relationName { get; set; }
        //public string? caption { get; set; }
        //public string? groupName { get; set; }
        public _self _self { get; set; }

        public void setDtoFromGiftCard(GiftCard giftCard)
        {
            this.id = giftCard.siesa_id;
            this.provider = "middleware";
            this.balance = giftCard.balance;
            this.totalBalance = giftCard.balance;
            this.relationName = giftCard.name;
            //this.caption = "";
            //this.groupName = "";
            this._self = new _self();
            this._self.href = "api/giftcards/" + giftCard.siesa_id;
        }

    }
}
