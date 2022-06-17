namespace colanta_backend.App.GiftCards.Controllers
{
    public class TransactionSummaryDto
    {
        public string cardId { get; set; }
        public string id  { get; set; }
        public _self _self { get; set; }

        public TransactionSummaryDto()
        {
            id = "642e_21a12_2022_06";
            _self.href = "";
        }

        public void setCardId(string cardId)
        {
            this.cardId = cardId;
        }
    }

    public class _self
    {
        public string href { get; set; }
    }
}
