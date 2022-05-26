namespace colanta_backend.App.GiftCards.Domain { 
    using System.Threading.Tasks;
    public interface GiftCardsRepository
    {
        Task<GiftCard[]> getAllGiftCards();
        Task<GiftCard[]> getGiftCardsByDocumentAndBusiness(string ownerDocument, string business);
        Task<GiftCard> getGiftCardBySiesaId(string siesaId);
        Task<GiftCard> saveGiftCard(GiftCard giftCard);
        Task<GiftCard> updateGiftCard(GiftCard giftCard);
        Task<GiftCard[]> updateGiftCards(GiftCard[] giftCards);
    }
}
