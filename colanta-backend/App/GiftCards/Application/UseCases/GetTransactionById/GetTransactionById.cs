namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class GetTransactionById
    {
        private GiftCardsRepository localRepository;

        public GetTransactionById(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Transaction> Invoke(string transactionId) 
        {
            return await this.localRepository.getTransaction(transactionId);
        }
    }
}
