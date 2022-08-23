namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class WayToPay
    {
        [JsonPropertyName("C263FormaPago")]
        public string wayToPay { get; set; }

        [JsonPropertyName("C263ReferenciaPago")]
        public string payRef { get; set; }

        [JsonPropertyName("C263Valor")]
        public decimal value { get; set; }
    }
}
