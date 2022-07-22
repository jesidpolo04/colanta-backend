namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public class GetTransactionSettlements
    {
        private GiftCardsRepository localRepository;

        public GetTransactionSettlements(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<TransactionSettlement[]> Invoke(string transactionId)
        {
            return await this.localRepository.GetTransactionSettlements(transactionId);
        }
    }
}
