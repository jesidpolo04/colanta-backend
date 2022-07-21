namespace colanta_backend.App.GiftCards.Domain
{
    using System.Threading.Tasks;
    public interface GiftCardsSiesaRepository
    {
        Task<GiftCard[]> getAllGiftCards();
        Task<GiftCard[]> getGiftCardsByDocumentAndBusiness(string document, string business);
        Task<decimal> getGiftCardBalanceBySiesaId(string siesaId);
    }
}
