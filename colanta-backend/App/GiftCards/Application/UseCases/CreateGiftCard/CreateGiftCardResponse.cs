namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    public class CreateGiftCardResponse
    {
        public string id { get; set; }
        public string redemptionToken { get; set; }
        public string redemptionCode { get; set; }
        public decimal balance { get; set; }
        public string relationName { get; set; }
        public string emissionDate { get; set; }
        public string expiringDate { get; set; }
        public string caption { get; set; }
        public string provider { get; set; }
        public string groupName { get; set; }
        public CreateGiftCardResponseTransaction transaction { get; set; }

        public void setFromGiftCard(GiftCard giftcard)
        {
            this.id = giftcard.siesa_id;
            this.redemptionCode = giftcard.code;
            this.redemptionToken = giftcard.token;
            this.balance = giftcard.balance;
            this.relationName = giftcard.name;
            this.emissionDate = giftcard.emision_date;
            this.expiringDate = giftcard.expire_date;
            this.caption = "";
            this.provider = "middleware";
            this.groupName = "";
            this.transaction = new CreateGiftCardResponseTransaction();
            this.transaction.href = $"giftcards/{this.id}/transactions";
        }
    }

    public class CreateGiftCardResponseTransaction
    {
        public string href { get; set; }
    }

}
