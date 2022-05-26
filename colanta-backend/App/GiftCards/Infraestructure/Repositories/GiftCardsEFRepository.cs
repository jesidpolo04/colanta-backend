using colanta_backend.App.GiftCards.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.GiftCards.Infraestructure
{
    using App.GiftCards.Domain;
    using App.Shared.Infraestructure;
    using App.Shared.Application;
    using App.Brands.Infraestructure;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    public class GiftCardsEFRepository : Domain.GiftCardsRepository
    {
        private ColantaContext dbContext;

        public GiftCardsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public async Task<GiftCard[]> getAllGiftCards()
        {
            EFGiftCard[] efGiftCards = this.dbContext.GiftCards.ToArray();
            List<GiftCard> giftCards = new List<GiftCard>();
            foreach (EFGiftCard efGiftCard in efGiftCards)
            {
                giftCards.Add(efGiftCard.getGiftCardFromEfGiftCard());
            }
            return giftCards.ToArray();
        }

        public async Task<GiftCard> getGiftCardBySiesaId(string siesaId)
        {
            var efGiftCards = this.dbContext.GiftCards.Where(giftcard => giftcard.siesa_id == siesaId);
            if(efGiftCards.ToArray().Length > 0)
            {
                return efGiftCards.First().getGiftCardFromEfGiftCard();
            }
            return null;
        }

        public async Task<GiftCard[]> getGiftCardsByDocumentAndBusiness(string ownerDocument, string business)
        {
            EFGiftCard[] efGiftCards = this.dbContext.GiftCards.Where(giftcard => giftcard.owner == ownerDocument && giftcard.business == business).ToArray();
            List<GiftCard> giftCards = new List<GiftCard>();
            foreach(EFGiftCard efGiftCard in efGiftCards)
            {
                giftCards.Add(efGiftCard.getGiftCardFromEfGiftCard());
            }
            return giftCards.ToArray();
        }

        public async Task<GiftCard> saveGiftCard(GiftCard giftCard)
        {
            EFGiftCard efGiftCard = new EFGiftCard();
            efGiftCard.setEfGiftCardFromGiftCard(giftCard);
            this.dbContext.Add(efGiftCard);
            this.dbContext.SaveChanges();
            return await this.getGiftCardBySiesaId(giftCard.siesa_id);
        }

        public async Task<GiftCard> updateGiftCard(GiftCard giftCard)
        {
            EFGiftCard efGiftCard = this.dbContext.GiftCards.Where(giftcard => giftcard.siesa_id == giftCard.siesa_id).First();
            efGiftCard.name = giftCard.name;
            efGiftCard.code = giftCard.code;
            efGiftCard.token = giftCard.token;
            efGiftCard.business = giftCard.business;
            efGiftCard.emision_date = giftCard.emision_date;
            efGiftCard.expire_date = giftCard.expire_date;
            efGiftCard.balance = giftCard.balance;
            efGiftCard.siesa_id = giftCard.siesa_id;

            this.dbContext.SaveChanges();
            return giftCard;
        }

        public async Task<GiftCard[]> updateGiftCards(GiftCard[] giftCards)
        {
            foreach(GiftCard giftCard in giftCards)
            {
                EFGiftCard efGiftCard = this.dbContext.GiftCards.Where(giftcard => giftcard.siesa_id == giftCard.siesa_id).First();
                efGiftCard.name = giftCard.name;
                efGiftCard.code = giftCard.code;
                efGiftCard.token = giftCard.token;
                efGiftCard.business = giftCard.business;
                efGiftCard.emision_date = giftCard.emision_date;
                efGiftCard.expire_date = giftCard.expire_date;
                efGiftCard.balance = giftCard.balance;
                efGiftCard.siesa_id = giftCard.siesa_id;
            }
            this.dbContext.SaveChanges();
            return giftCards;
        }
    }
}
