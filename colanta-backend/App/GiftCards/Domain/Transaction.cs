namespace colanta_backend.App.GiftCards.Domain
{
    using System;
    public class Transaction
    {
        public string id { get; set; }
        public decimal value { get; set; }
        public GiftCard card { get; set; }
        public int card_id { get; set; }
        public string json { get; set; }
        public TransactionAuthorization transaction_authorization { get; set; }
        public string transaction_authorization_id { get; set; }
        public DateTime date { get; set; }

        public Transaction(int card_id, decimal value, string transactionJson) 
        {
            this.id = Guid.NewGuid().ToString();
            this.card_id = card_id;
            this.json = transactionJson;
            this.date = DateTime.Now;
            this.transaction_authorization = this.authorize();
        }

        public Transaction()
        {
        }

        public TransactionAuthorization authorize()
        {
            return new TransactionAuthorization(this.value, this);
        }

        public TransactionCancellation cancel(decimal value)
        {
            return new TransactionCancellation(value, this);
        }

        public TransactionSettlement generateSettlement(decimal value)
        {
            return new TransactionSettlement(value, this);
        }
    }
}
