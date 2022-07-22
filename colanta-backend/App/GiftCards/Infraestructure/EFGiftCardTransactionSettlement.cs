namespace colanta_backend.App.GiftCards.Infraestructure
{
    using System;
    using GiftCards.Domain;
    public class EFGiftCardTransactionSettlement
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public EFGiftCardTransaction transaction { get; set; }
        public string transaction_id { get; set; }
        public DateTime date { get; set; }

        public TransactionSettlement getTransactionSettlement()
        {
            TransactionSettlement transactionSettlement = new TransactionSettlement();
            transactionSettlement.oid = oid;
            transactionSettlement.value = value;
            transactionSettlement.date = date;
            transactionSettlement.transaction = transaction.getTransaction();
            return transactionSettlement;
        }

        public void setEfTransactionSettlement(TransactionSettlement transactionSettlement)
        {
            this.date = transactionSettlement.date;
            this.value = transactionSettlement.value;
            this.oid = transactionSettlement.oid;
            this.transaction_id = transactionSettlement.transaction.id;
        }
    }
}
