namespace colanta_backend.App.GiftCards.Domain { 
    using System.Threading.Tasks;
    public interface GiftCardsRepository
    {
        Task<GiftCard[]> getAllGiftCards();
        Task<GiftCard[]> getGiftCardsByDocumentAndBusiness(string document, string business);
        Task<GiftCard[]> getGiftCardsByDocumentAndEmail(string document, string email);
        Task<GiftCard> getGiftCardBySiesaId(string siesaId);
        Task<GiftCard> saveGiftCard(GiftCard giftCard);
        Task<GiftCard> updateGiftCard(GiftCard giftCard);
        Task<GiftCard[]> updateGiftCards(GiftCard[] giftCards);

        Task<Transaction> saveGiftCardTransaction(Transaction transaction);
        Task<TransactionAuthorization> saveTransactionAuthorization(TransactionAuthorization transactionAuthorization);
        Task<TransactionCancellation> saveTransactionCancellation(TransactionCancellation transactionCancellation); 
        Task<TransactionSettlement> saveTransactionSettlement(TransactionSettlement transactionSettlement);

        Task<Transaction> getTransaction(string transactionId);
        Task<Transaction[]> getGiftCardTransactions(int giftCardId);
        Task<TransactionAuthorization> getTransactionAuthorization(string transactionId);
        Task<TransactionCancellation[]> getTransactionCancellations(string transactionId);
        Task<TransactionSettlement[]> GetTransactionSettlements(string transactionId);

    }
}
