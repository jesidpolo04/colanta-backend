namespace colanta_backend.App.Prices.Domain
{
    using System.Threading.Tasks;

    public interface PricesVtexRepository
    {
        Task<Price> getPriceByVtexId(int? vtexId);
        Task<Price> savePrice(Price price);
        Task<Price> updatePrice(Price price);
    }
}
