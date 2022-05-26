namespace colanta_backend.App.GiftCards.Infraestructure
{
    using GiftCards.Domain;
    public class EFGiftCard
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string token { get; set; }
        public string business { get; set; }
        public decimal balance { get; set; }
        public string owner { get; set; }
        public string emision_date { get; set; }
        public string expire_date { get; set; }

        public GiftCard getGiftCardFromEfGiftCard()
        {
            GiftCard giftCard = new GiftCard();
            giftCard.id = this.id;
            giftCard.name = this.name;
            giftCard.siesa_id = this.siesa_id;
            giftCard.code = this.code;
            giftCard.token = this.token;
            giftCard.balance = this.balance;
            giftCard.owner = this.owner;
            giftCard.emision_date = this.emision_date;
            giftCard.expire_date = this.expire_date;
            giftCard.business = this.business;
            return giftCard;
        }

        public void setEfGiftCardFromGiftCard(GiftCard giftCard)
        {
            this.id = giftCard.id;
            this.name = giftCard.name;
            this.siesa_id = giftCard.siesa_id;
            this.code = giftCard.code;
            this.token = giftCard.token;
            this.balance = giftCard.balance;
            this.owner = giftCard.owner;
            this.emision_date = giftCard.emision_date;
            this.expire_date = giftCard.expire_date;
            this.business = giftCard.business;
        }
    }
}
