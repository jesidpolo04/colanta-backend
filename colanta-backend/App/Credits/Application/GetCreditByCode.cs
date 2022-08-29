namespace colanta_backend.App.Credits.Application
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using GiftCards.Domain;
    public class GetCreditByCode
    {
        private GiftCardsRepository localGiftcardsRepository;

        public GetCreditByCode(GiftCardsRepository localGiftcardsRepository)
        {
            this.localGiftcardsRepository = localGiftcardsRepository;
        }

        public Task<GiftCard> Invoke(string redemptionCode)
        {
            return this.localGiftcardsRepository.getGiftCardBySiesaId(redemptionCode); 
        }
    }
}
