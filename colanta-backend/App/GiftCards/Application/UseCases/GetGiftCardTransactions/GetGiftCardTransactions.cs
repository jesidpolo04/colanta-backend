namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public class GetGiftCardTransactions
    {
        private GiftCardsRepository localRepository;

        public GetGiftCardTransactions(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<Transaction[]> Invoke(string giftCardId)
        {
            return await this.localRepository.getGiftCardTransactions(giftCardId);
        }
    }
}
