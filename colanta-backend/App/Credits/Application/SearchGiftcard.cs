namespace colanta_backend.App.Credits.Application
{
    using System.Linq;
    using GiftCards.Domain;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class SearchGiftcard
    {
        private GiftCardsRepository giftCardsRepository;

        public SearchGiftcard(GiftCardsRepository giftCardsRepository)
        {
            this.giftCardsRepository = giftCardsRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string email, string code, string someSkuVtexRef)
        {
            string business = "mercolanta";
            GiftCard[] availableCodes = getLocalAvailableCodes(document, email, code, business).ToArray();
            if (availableCodes.Length > 0)
            {
                return new GiftCard[1] { availableCodes.First() };
            }
            return new GiftCard[0] { };
        }

        private List<GiftCard> getLocalAvailableCodes(string document, string email, string code, string business)
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
            return availableCodes;
        }
    }
}
