using Newtonsoft.Json;

namespace colanta_backend.App.PriceTables{
    public class FixedPriceDto{

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("listPrice")]
        public decimal ListPrice { get; set; }
        
        [JsonProperty("minQuantity")]
        public int MinQuantity { get; set; }

        public static FixedPriceDto GetDtoFromFixedPrice(FixedPrice fixedPrice){
            return new FixedPriceDto{
                ListPrice = fixedPrice.ListPrice,
                MinQuantity = fixedPrice.MinQuantity,
                Value = fixedPrice.Value
            };
        }
    }
}