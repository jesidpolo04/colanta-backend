namespace colanta_backend.App.Credits.Application
{
    using System;
    using System.Linq;
    using Credits.Domain;
    using GiftCards.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class SearchCredit
    {
        private GiftCardsRepository giftCardsRepository;
        private CreditsSiesaRepository creditsSiesaRepository;
        private SiesaOrdersRepository siesaOrdersRepository;

        public SearchCredit(GiftCardsRepository giftCardsRepository)
        {
            this.giftCardsRepository = giftCardsRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string email, string code)
        {
            GiftCard[] availableCodes = getLocalAvailableCodes(document, email, code);
            if (availableCodes.Length > 0)
            {
                return new GiftCard[1] { availableCodes.First() };
            }
            return new GiftCard[0] { };
        }

        private GiftCard[] getLocalAvailableCodes(string document, string email, string code)
        {
            DateTime now = DateTime.Now;
            List<GiftCard> availableCodes = new List<GiftCard>();
            GiftCard[] userGiftcards = this.giftCardsRepository.getGiftCardsByDocumentAndEmail(document, email).Result;
            foreach (GiftCard giftCard in userGiftcards)
            {
                DateTime giftcardExpireDate = DateTime.Parse(giftCard.expire_date);
                if (DateTime.Compare(giftcardExpireDate, now) >= 0 &&
                    giftCard.used == false &&
                    giftCard.provider == Providers.CUPO &&
                    giftCard.code == code
                    )
                {
                    availableCodes.Add(giftCard);
                }
            }
            return availableCodes.ToArray();
        }
    }
}
