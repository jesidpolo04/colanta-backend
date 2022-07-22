namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public class GetTransactionCancellations
    {
        private GiftCardsRepository localRepository;

        public GetTransactionCancellations(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<TransactionCancellation[]> Invoke(string transactionId)
        {
            return await this.localRepository.getTransactionCancellations(transactionId);
        }
    }
}
