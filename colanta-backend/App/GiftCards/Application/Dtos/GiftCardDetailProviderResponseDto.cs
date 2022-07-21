namespace colanta_backend.App.GiftCards.Application
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
        public string provider {get; set;}
        public TransactionDto transaction { get; set; }

        public GiftCardDetailProviderResponseDto()
        {
            this.provider = "middleware";
        }


        public void setDtoFromGiftCard(GiftCard giftCard)
        {
            this.id = giftCard.siesa_id;
            this.redemptionToken = giftCard.token;
            this.redemptionCode = giftCard.code;
            this.balance = giftCard.balance;
            this.relationName = giftCard.name;
            this.emissionDate = giftCard.emision_date;
            this.expiringDate = giftCard.expire_date;
            this.transaction = new TransactionDto();
            this.transaction.href = $"/api/giftcards/{id}/transactions";
        }
    }

    public class TransactionDto {
        public string href { get; set; }
    }
}
