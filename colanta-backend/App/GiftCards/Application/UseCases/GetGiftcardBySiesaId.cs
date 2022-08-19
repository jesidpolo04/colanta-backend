namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Threading.Tasks;
    public class GetGiftcardBySiesaId
    {
        private GiftCardsRepository localRepository;
        private GiftCardsSiesaRepository siesaRepository;
        public GetGiftcardBySiesaId(
            GiftCardsRepository localRepository,
            GiftCardsSiesaRepository siesaRepository
)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task<GiftCard> Invoke(string siesaId)
        {
             
            return await localRepository.getGiftCardBySiesaId(siesaId);
        }
    }
}
