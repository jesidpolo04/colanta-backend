namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using System.Linq;
    public class SearchGiftcards
    {
        private GiftCardsSiesaRepository siesaRepository;
        private GiftCardsRepository localRepository;
        public SearchGiftcards(GiftCardsRepository localRepository,GiftCardsSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
            this.localRepository = localRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string business, string redemptionCode)
        {
            GiftCard[] siesaGiftCards = await this.siesaRepository.getGiftCardsByDocumentAndBusiness(document, business);
            foreach(GiftCard siesaGiftCard in siesaGiftCards)
            {
                GiftCard localGiftCard = await localRepository.getGiftCardBySiesaId(siesaGiftCard.siesa_id);
                if (localGiftCard == null)
                {
                    await localRepository.saveGiftCard(siesaGiftCard);
                }
            }
            GiftCard[] localGiftcards = await this.localRepository.getGiftCardsByDocumentAndBusiness(document, business);
            return localGiftcards.Where(giftcard => giftcard.code == redemptionCode).ToArray();
        }
    }
}
