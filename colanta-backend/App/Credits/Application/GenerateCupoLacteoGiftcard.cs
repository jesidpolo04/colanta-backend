namespace colanta_backend.App.Credits.Application
{
    using System;
    using System.Linq;
    using Credits.Domain;
    using GiftCards.Domain;
    using Orders.SiesaOrders.Domain;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class GenerateCupoLacteoGiftcard
    {
        private GiftCardsRepository giftCardsRepository;
        private CupoLacteoSiesaRepository creditsSiesaRepository;
        private SiesaOrdersRepository siesaOrdersRepository;

        public GenerateCupoLacteoGiftcard(GiftCardsRepository giftCardsRepository, CupoLacteoSiesaRepository creditsSiesaRepository, SiesaOrdersRepository siesaOrdersRepository)
        {
            this.giftCardsRepository = giftCardsRepository;
            this.creditsSiesaRepository = creditsSiesaRepository;
            this.siesaOrdersRepository = siesaOrdersRepository;
        }

        public async Task<GiftCard?> Invoke(string document, string email, string business)
        {
            this.inactivateCurrentAvailableGiftcards(document, email, business);
            GiftCard newGiftcard = await this.creditsSiesaRepository.getCupoLacteo(document, email, business);
            if(newGiftcard == null) return null;
            await this.giftCardsRepository.saveGiftCard(newGiftcard);
            return newGiftcard;
        }

        private GiftCard[] getLocalAvailableGiftcards(string document, string email, string business)
        {
            List<GiftCard> availableCodes = new List<GiftCard>();
            GiftCard[] userGiftcards = this.giftCardsRepository.getGiftCardsByDocumentAndEmail(document, email).Result;
            foreach(GiftCard giftCard in userGiftcards)
            {
                if( !giftCard.isExpired() && !giftCard.used && giftCard.business == business && giftCard.provider == Providers.CUPO)
                {
                    availableCodes.Add(giftCard);
                }
            }
            return availableCodes.ToArray();
        }

        private void inactivateCurrentAvailableGiftcards(string document, string email, string business)
        {
            GiftCard[] availableGiftcards = this.getLocalAvailableGiftcards(document, email, business);
            foreach(GiftCard giftcard in availableGiftcards)
            {
                giftcard.used = true;
                this.giftCardsRepository.updateGiftCard(giftcard);
            }
        }
    }
}
