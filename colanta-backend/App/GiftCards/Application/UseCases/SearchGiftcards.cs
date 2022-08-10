namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using Products.Domain;
    using System.Linq;
    public class SearchGiftcards
    {
        private GiftCardsSiesaRepository siesaRepository;
        private GiftCardsRepository localRepository;
        private SkusRepository skusLocalRepository;
        public SearchGiftcards(GiftCardsRepository localRepository,GiftCardsSiesaRepository siesaRepository, SkusRepository skusLocalRepository)
        {
            this.siesaRepository = siesaRepository;
            this.localRepository = localRepository;
            this.skusLocalRepository = skusLocalRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string skuRefId, string redemptionCode)
        {
            Sku sku = skusLocalRepository.getSkuByConcatSiesaId(skuRefId).Result;
            string business = sku != null ? sku.product.business : "";
            
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
