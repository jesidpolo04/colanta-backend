namespace colanta_backend.App.Credits.Domain
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public interface CupoLacteoSiesaRepository
    {
        Task<GiftCard> getCupoLacteo(string document, string email, string business);
    }
}
