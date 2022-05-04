namespace colanta_backend.App.Prices.Domain

{
    using System.Threading.Tasks;

    public interface PricesSiesaRepository
    {
        Task<Price[]> getAllPrices();
    }
}
