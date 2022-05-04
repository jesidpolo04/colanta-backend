namespace colanta_backend.App.Prices.Infraestructure
{
    using Prices.Domain;
    using System;
    public class GetPriceDto
    {
        public string itemId { get; set; }
        public decimal? listPrice { get; set; }
        public decimal costPrice { get; set; }
        public decimal markup { get; set; }
        public decimal basePrice { get; set; }
        public object[] fixedPrices { get; set; }

        public Price getPriceFromDto()
        {
            Price price = new Price();

            price.price = basePrice;
            return price;
        }
    }
}
