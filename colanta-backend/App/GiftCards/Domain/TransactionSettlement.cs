namespace colanta_backend.App.GiftCards.Domain
{
    using System;
    public class TransactionSettlement
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public Transaction transaction { get; set; }
        public DateTime date { get; set; }

        public TransactionSettlement(decimal value, Transaction transaction)
        {
            this.oid = Guid.NewGuid().ToString();
            this.value = value;
            this.transaction = transaction;
            this.date = DateTime.Now;
        }
        public TransactionSettlement()
        {
        }
    }
}
