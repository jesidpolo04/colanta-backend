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
            Transaction transaction = await this.localRepository.getTransaction(transactionId);
            TransactionCancellation transactionCancellation = transaction.cancel(value);
            transactionCancellation = await this.localRepository.saveTransactionCancellation(transactionCancellation);
            return transactionCancellation;
        }
    }
}
