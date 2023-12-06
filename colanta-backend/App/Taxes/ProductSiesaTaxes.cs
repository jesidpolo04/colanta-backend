namespace colanta_backend.App.Taxes
{
    using Newtonsoft.Json;
    public class ProductSiesaTaxes
    {
        [JsonProperty("id_producto")]
        public string IdProducto { get; set; }

        [JsonProperty("negocio")]
        public string Negocio { get; set; }

        [JsonProperty("id_variacion")]
        public string? IdVariacion { get; set; }

        [JsonProperty("impuesto_saludable_porcentual")]
        public decimal ImpuestoSaludablePorcentual { get; set; }

        [JsonProperty("impuesto_saludable_nominal")]
        public decimal ImpuestoSaludableNominal { get; set; }

        [JsonProperty("impuesto_consumo_nominal")]
        public decimal ImpuestoConsumoNominal { get; set; }

        [JsonProperty("iva")]
        public decimal Iva { get; set; }
    }
}