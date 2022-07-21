namespace colanta_backend.App.GiftCards.Application
{
    public class TransactionSummaryDto
    {
        public string cardId { get; set; }
        public string id  { get; set; }
        public _self _self { get; set; }

        public TransactionSummaryDto(string siesaCardId, string transactionId)
        {
            this.id = id;
            this.cardId = siesaCardId;
            _self = new _self();
            _self.href = $"/giftcards/{siesaCardId}/transactions/{transactionId}";
        }
    }

    public class _self
    {
        public string href { get; set; }
    }
}
