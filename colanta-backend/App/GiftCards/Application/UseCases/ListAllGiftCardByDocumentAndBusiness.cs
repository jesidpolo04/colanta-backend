namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public class ListAllGiftCardByDocumentAndBusiness
    {
        private GiftCardsSiesaRepository siesaRepository;
        private GiftCardsRepository localRepository;
        public ListAllGiftCardByDocumentAndBusiness(GiftCardsRepository localRepository,GiftCardsSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
            this.localRepository = localRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string business)
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
            return await this.localRepository.getGiftCardsByDocumentAndBusiness(document, business);
        }
    }
}
