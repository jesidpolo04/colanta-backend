namespace colanta_backend.App.OrderObservations.Controllers
{
    using System.Text.Json.Serialization;
    using colanta_backend.App.OrderObservations.Domain;
    
    public class ObservationFieldDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public void SetDto(ProductObservationField field)
        {
            this.Id = field.Id;
            this.Code = field.Code;
            this.Description = field.Description;
        }
    }
}