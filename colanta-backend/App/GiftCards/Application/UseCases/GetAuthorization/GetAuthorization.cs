namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Threading.Tasks;
    public class GetAuthorization
    {
        private GiftCardsRepository localRepository;

        public GetAuthorization(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<TransactionAuthorization> Invoke(string transactionId)
        {
             return await this.localRepository.getTransactionAuthorization(transactionId);
        }
    }
}
