namespace colanta_backend.App.GiftCards.Infraestructure
{
    using System;
    using GiftCards.Domain;
    public class EFGiftCardTransaction
    {
        public string id { get; set; }
        public decimal value { get; set; }
        public EFGiftCard card { get; set; }
        public int card_id { get; set; }
        public string json { get; set; }
        public EFGiftCardTransactionAuthorization transaction_authorization { get; set; }
        public string transaction_authorization_id { get; set; }
        public DateTime date { get; set; }

        public Transaction getTransaction()
        {
            Transaction transaction = new Transaction();
            transaction.id = this.id;
            transaction.value = this.value;
            transaction.card_id = this.card_id;
            transaction.json = this.json;
            transaction.transaction_authorization_id = this.transaction_authorization_id;
            transaction.date = this.date;

            transaction.card = this.card.getGiftCardFromEfGiftCard();
            transaction.transaction_authorization = this.transaction_authorization.getTransactionAuthorization();

            return transaction;
        }

        public void setEfTransaction(Transaction transaction)
        {
            this.id = transaction.id;
            this.value = transaction.value;
            this.card_id = transaction.card_id;
            this.json = transaction.json;
            this.transaction_authorization_id = transaction.transaction_authorization_id;
            this.date = transaction.date;

            this.card = new EFGiftCard();
            this.card.setEfGiftCardFromGiftCard(transaction.card);

            EFGiftCardTransactionAuthorization efTransactionAuthorization = new EFGiftCardTransactionAuthorization();
            efTransactionAuthorization.date = transaction.transaction_authorization.date;
            efTransactionAuthorization.value = transaction.transaction_authorization.value;
            efTransactionAuthorization.oid = transaction.transaction_authorization.oid;
            this.transaction_authorization = efTransactionAuthorization;
        }
    }
}
