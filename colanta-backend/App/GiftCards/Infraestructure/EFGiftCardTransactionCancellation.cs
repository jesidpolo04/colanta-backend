namespace colanta_backend.App.GiftCards.Infraestructure
{
    using System;
    using GiftCards.Domain;
    public class EFGiftCardTransactionCancellation
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public EFGiftCardTransaction transaction { get; set; }
        public DateTime date { get; set; }

        public TransactionCancellation getTransactionCancellation()
        {
            TransactionCancellation transactionCancellation = new TransactionCancellation();
            transactionCancellation.oid = oid;
            transactionCancellation.value = value;
            transactionCancellation.date = date;
            transactionCancellation.transaction = transaction.getTransaction();
            return transactionCancellation;
        }

        public void setEfTransactionCancellation(TransactionCancellation transactionCancellation)
        {
            this.date = transactionCancellation.date;
            this.value = transactionCancellation.value;
            this.oid = transactionCancellation.oid;
            this.transaction = new EFGiftCardTransaction();
            this.transaction.setEfTransaction(transactionCancellation.transaction);
        }
    }
}
