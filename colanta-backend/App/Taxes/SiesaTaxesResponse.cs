namespace colanta_backend.App.Taxes{
    using Newtonsoft.Json;
    public class SiesaTaxesResponse{
        [JsonProperty("impuestos")]
        public ProductSiesaTaxes[] Impuestos;
    }
}