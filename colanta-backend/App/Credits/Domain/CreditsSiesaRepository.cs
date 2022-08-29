namespace colanta_backend.App.Credits.Domain
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public interface CreditsSiesaRepository
    {
        Task<GiftCard> getCreditByDocumentAndEmail(string document, string email);
    }
}
