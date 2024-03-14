using Newtonsoft.Json;

namespace colanta_backend.App.Bags {
    public class ErpBag {
        [JsonProperty("id_producto")]
        public string IdProducto { get; set; } //Referencia ERP
        [JsonProperty("peso")]
        public float Peso { get; set; } //Capacidad
    }
}