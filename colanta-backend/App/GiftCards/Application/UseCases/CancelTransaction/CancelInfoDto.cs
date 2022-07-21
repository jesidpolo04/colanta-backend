namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    public class CancelInfoDto
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public string date { get; set; }

        public void setFromTransactionCancellation(TransactionCancellation transactionCancellation)
        {
            this.oid = transactionCancellation.oid;
            this.value = transactionCancellation.value;
            this.date = transactionCancellation.date.ToUniversalTime().ToString();
        }
    }
}
