namespace colanta_backend.App.Taxes
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class TaxesResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("taxes")]
        public List<Tax> Taxes { get; set; }
    }

    public class Tax
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; }
        
    }
}