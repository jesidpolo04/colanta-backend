namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Threading.Tasks;
    public class SettlementTransaction
    {
        private GiftCardsRepository localRepository;

        public SettlementTransaction(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<TransactionSettlement> Invoke(string transactionId, decimal value)
        {
            Transaction transaction = await this.localRepository.getTransaction(transactionId);
            TransactionSettlement settlement = transaction.generateSettlement(value);
            settlement = await this.localRepository.saveTransactionSettlement(settlement);
            return settlement;
        }
    }
}
