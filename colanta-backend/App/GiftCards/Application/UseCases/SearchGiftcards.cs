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
            if (redemptionCode == "" || redemptionCode == null) return new GiftCard[0]{};
            string business = this.getBusiness(skuRefId);
            GiftCard[] siesaGiftCards = await this.siesaRepository.getGiftCardsByDocumentAndBusiness(document, business);

            foreach(GiftCard siesaGiftCard in siesaGiftCards)
            {
                GiftCard localGiftCard = await localRepository.getGiftCardBySiesaId(siesaGiftCard.siesa_id);
                if (localGiftCard == null)
                {
                    await localRepository.saveGiftCard(siesaGiftCard);
                }
                if(localGiftCard != null && !(this.userHasPendingOrders(document)))
                {
                    this.updateGiftcardBalance(localGiftCard);
                }
            }

            GiftCard[] localGiftcards = await this.localRepository.getGiftCardsByDocumentAndBusiness(document, business);
            return localGiftcards.Where(giftcard => giftcard.code == redemptionCode).ToArray();
        }

        private string getBusiness(string someSkuRefId)
        {
            Sku sku = skusLocalRepository.getSkuByConcatSiesaId(someSkuRefId).Result;
            return sku != null ? sku.product.business : "";
        }

        private bool userHasPendingOrders(string document)
        {
            SiesaOrder[] userOrders = this.siesaOrdersLocalRepository.getSiesaOrdersByDocument(document).Result;
            SiesaOrder[] unfinishedUserOrder = userOrders.Where(siesaOrder => siesaOrder.finalizado == false).ToArray();
            return unfinishedUserOrder.Length > 0 ? true : false;
        }

        private void updateGiftcardBalance(GiftCard localGiftcard)
        {
            decimal newCardBalance = this.siesaRepository.getGiftCardBalanceBySiesaId(localGiftcard.siesa_id).Result;
            localGiftcard.updateBalance(newCardBalance);
            this.localRepository.updateGiftCard(localGiftcard).Wait();
        }
    }
}
