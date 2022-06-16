namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Threading.Tasks;
    public class GetAndUpdateGiftCardBySiesaId
    {
        private GiftCardsRepository localRepository;
        private GiftCardsSiesaRepository siesaRepository;
        public GetAndUpdateGiftCardBySiesaId(
            GiftCardsRepository localRepository,
            GiftCardsSiesaRepository siesaRepository
)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task<GiftCard> Invoke(string siesaId)
        {
            GiftCard localGiftCard = await localRepository.getGiftCardBySiesaId(siesaId);
            decimal newBalance = await siesaRepository.getGiftCardBalanceBySiesaId(siesaId);
            localGiftCard.balance = newBalance;
            await localRepository.updateGiftCard(localGiftCard);
            return localGiftCard;
        }
    }
}
