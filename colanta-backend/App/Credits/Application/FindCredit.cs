namespace colanta_backend.App.Credits.Application
{
    using System;
    using System.Linq;
    using Credits.Domain;
    using GiftCards.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class FindCredit
    {
        private GiftCardsRepository giftCardsRepository;
        private CreditsSiesaRepository creditsSiesaRepository;
        private SiesaOrdersRepository siesaOrdersRepository;

        public FindCredit(GiftCardsRepository giftCardsRepository, CreditsSiesaRepository creditsSiesaRepository, SiesaOrdersRepository siesaOrdersRepository)
        {
            this.giftCardsRepository = giftCardsRepository;
            this.creditsSiesaRepository = creditsSiesaRepository;
            this.siesaOrdersRepository = siesaOrdersRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string email)
        {
            GiftCard[] availableCodes = getLocalAvailableCodes(document, email);
            if(availableCodes.Length > 0)
            {
                return new GiftCard[1] {availableCodes.First()};
            }
            GiftCard newCode = await this.creditsSiesaRepository.getCreditByDocumentAndEmail(document, email);
            if (newCode == null) return new GiftCard[0] { };
            await this.giftCardsRepository.saveGiftCard(newCode);
            return new GiftCard[1] {newCode};
        }

        private GiftCard[] getLocalAvailableCodes(string document, string email)
        {
            DateTime now = DateTime.Now;
            List<GiftCard> availableCodes = new List<GiftCard>();
            GiftCard[] userGiftcards = this.giftCardsRepository.getGiftCardsByDocumentAndEmail(document, email).Result;
            foreach(GiftCard giftCard in userGiftcards)
            {
                DateTime giftcardExpireDate = DateTime.Parse(giftCard.expire_date);
                if(DateTime.Compare(giftcardExpireDate, now) >= 0 && 
                    giftCard.used == false && 
                    giftCard.provider == Providers.CUPO)
                {
                    availableCodes.Add(giftCard);
                }
            }
            return availableCodes.ToArray();
        }
    }
}
