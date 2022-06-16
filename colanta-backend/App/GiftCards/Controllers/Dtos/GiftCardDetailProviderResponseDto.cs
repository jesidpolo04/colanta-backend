namespace colanta_backend.App.GiftCards.Controllers
{
    using GiftCards.Domain;
    public class GiftCardDetailProviderResponseDto
    {
        public string id { get; set; }
        public string redemptionToken { get; set; }
        public string redemptionCode { get; set; }
        public decimal balance { get; set; }
        public string relationName { get; set; }
        public string emissionDate { get; set; }
        public string expiringDate { get; set; }
        public string provider = "middleware";
        public TransactionDto transaction { get; set; }

        public void setDtoFromGiftCard(GiftCard giftCard)
        {
            this.id = giftCard.siesa_id;
            this.redemptionCode = giftCard.token;
            this.redemptionCode = giftCard.code;
            this.balance = giftCard.balance;
            this.relationName = giftCard.name;
            this.emissionDate = giftCard.emision_date;
            this.expiringDate = giftCard.expire_date;
        }
    }

    public class TransactionDto {
        public string href = "";
    }
}
