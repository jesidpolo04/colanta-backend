namespace colanta_backend.App.Credits.Application
{
    using System;
    using System.Linq;
    using Credits.Domain;
    using GiftCards.Domain;
    using Products.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class SearchGiftcard
    {
        private GiftCardsRepository giftCardsRepository;
        private SkusRepository skusRepository;

        public SearchGiftcard(GiftCardsRepository giftCardsRepository, SkusRepository skusRepository)
        {
            this.giftCardsRepository = giftCardsRepository;
            this.skusRepository = skusRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string email, string code, string someSkuVtexRef)
        {
            string business = this.getBusinessFromSomeSkuVtexRef(someSkuVtexRef);
            GiftCard[] availableCodes = getLocalAvailableCodes(document, email, code, business);
            if (availableCodes.Length > 0)
            {
                return new GiftCard[1] { availableCodes.First() };
            }
            return new GiftCard[0] { };
        }

        private GiftCard[] getLocalAvailableCodes(string document, string email, string code, string business)
        {
            List<GiftCard> availableCodes = new List<GiftCard>();
            GiftCard[] userGiftcards = this.giftCardsRepository.getGiftCardsByDocumentAndEmail(document, email).Result;
            foreach (GiftCard giftCard in userGiftcards)
            {
                if (!giftCard.isExpired() &&
                    !giftCard.used &&
                    giftCard.provider == Providers.CUPO &&
                    giftCard.code == code &&
                    giftCard.business == business
                    )
                {
                    availableCodes.Add(giftCard);
                }
            }
            return availableCodes.ToArray();
        }

        private string getBusinessFromSomeSkuVtexRef(string someSkuVtexRef)
        {
            Sku sku = this.skusRepository.getSkuByConcatSiesaId(someSkuVtexRef).Result;
            if (sku != null) return sku.product.business;
            else return "";
        }
    }
}
