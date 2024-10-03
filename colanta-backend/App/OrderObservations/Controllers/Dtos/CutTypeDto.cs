namespace colanta_backend.App.OrderObservations.Controllers{
    using System.Text.Json.Serialization;
    using colanta_backend.App.OrderObservations.Domain;

    public class CutTypeDto{
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public void SetDto(ProductCutTypeValue cutType){
            this.Id = cutType.Id;
            this.Code = cutType.Code;
            this.Description = cutType.Description;
        }
    }
}