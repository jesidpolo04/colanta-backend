namespace colanta_backend.App.GiftCards.Infraestructure
{
    using System;
    using GiftCards.Domain;
    public class EFGiftCardTransactionAuthorization
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public DateTime date { get; set; }
        public EFGiftCardTransaction transaction { get; set; }

        public TransactionAuthorization getTransactionAuthorization()
        {
            TransactionAuthorization transactionAuthorization = new TransactionAuthorization();
            transactionAuthorization.oid = oid;
            transactionAuthorization.value = value;
            transactionAuthorization.date = date;

            Transaction transaction = new Transaction();
            transaction.id = this.transaction.id;
            transaction.value = this.transaction.value;
            transaction.card_id = this.transaction.card_id;
            transaction.json = this.transaction.json;
            transaction.transaction_authorization_id = this.transaction.transaction_authorization_id;
            transaction.date = this.transaction.date;
            transaction.card = this.transaction.card.getGiftCardFromEfGiftCard();
            transactionAuthorization.transaction = transaction;

            return transactionAuthorization;
        }

        public void setEfTransactionAuthorization(TransactionAuthorization transactionAuthorization)
        {
            this.date = transactionAuthorization.date;
            this.value = transactionAuthorization.value;
            this.oid = transactionAuthorization.oid;
            this.transaction.setEfTransaction(transactionAuthorization.transaction);
        }
    }
}
