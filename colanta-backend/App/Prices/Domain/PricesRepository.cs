namespace colanta_backend.App.Prices.Domain
{
    using System.Threading.Tasks;
    using colanta_backend.App.Brands.Domain;

    public interface PricesRepository
    {
        public Task<Price> getPriceBySkuConcatSiesaId(string concat_siesa_id);
        public Task<Price> getPriceBySkuId(int sku_id);
        public Task<Price> savePrice(Price price);
        public Task<Price> updatePrice(Price price);
        public Task<Price[]> updatePrices(Price[] prices);
        public Price getPriceWithCategoryAndBrand(string concatSiesaId);

        public Price[] getPricesByBrand(int brandId);
        public Price[] getPricesByCategory(int categoryId);
        public Price[] getPricesByProduct(int productId);
        public Price[] getPricesBySkuIds(int[] skuIds);
    }
}
