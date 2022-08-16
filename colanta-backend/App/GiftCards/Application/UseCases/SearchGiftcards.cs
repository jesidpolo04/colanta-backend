namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using Products.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Linq;
    public class SearchGiftcards
    {
        private GiftCardsSiesaRepository siesaRepository;
        private GiftCardsRepository localRepository;
        private SkusRepository skusLocalRepository;
        private SiesaOrdersRepository siesaOrdersLocalRepository;
        public SearchGiftcards(
            GiftCardsRepository localRepository,
            GiftCardsSiesaRepository siesaRepository, 
            SkusRepository skusLocalRepository,
            SiesaOrdersRepository siesaOrdersLocalRepository
            )
        {
            this.siesaRepository = siesaRepository;
            this.localRepository = localRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.siesaOrdersLocalRepository = siesaOrdersLocalRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string skuRefId, string redemptionCode)
        {
            SiesaOrder[] userOrders = await this.siesaOrdersLocalRepository.getSiesaOrdersByDocument(document);
            SiesaOrder[] unfinishedUserOrder = userOrders.Where(siesaOrder => siesaOrder.finalizado == false).ToArray();

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
                if(localGiftCard != null && unfinishedUserOrder.Length == 0)
                {
                    decimal newCardBalance = await this.siesaRepository.getGiftCardBalanceBySiesaId(localGiftCard.siesa_id);
                    localGiftCard.updateBalance(newCardBalance);
                    this.localRepository.updateGiftCard(localGiftCard).Wait();
                }
            }

            GiftCard[] localGiftcards = await this.localRepository.getGiftCardsByDocumentAndBusiness(document, business);
            return localGiftcards.Where(giftcard => giftcard.code == redemptionCode).ToArray();
        }
    }
}
