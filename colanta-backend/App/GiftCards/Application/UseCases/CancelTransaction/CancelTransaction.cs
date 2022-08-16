namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Threading.Tasks;
    public class CancelTransaction
    {
        private GiftCardsRepository localRepository;

        public CancelTransaction(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<TransactionCancellation> Invoke(string transactionId, decimal value)
        {
            var transaction = await this.localRepository.getTransaction(transactionId);
            var transactionCancellation = transaction.cancel(value);
            var giftcard = transaction.card;
            decimal newBalance = giftcard.balance + value;
            giftcard.updateBalance(newBalance);
            transactionCancellation = await this.localRepository.saveTransactionCancellation(transactionCancellation);
            await localRepository.updateGiftCard(giftcard);
            return transactionCancellation;
        }
    }
}
