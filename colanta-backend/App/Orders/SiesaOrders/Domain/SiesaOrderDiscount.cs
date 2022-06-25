namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    public class SiesaOrderDiscount
    {
        public int id { get; set; }
        public string desto_co { get; set; }
        public string referencia_vtex { get; set; }
        public int nro_detalle { get; set; }
        public int orden_descuento { get; set; }
        public decimal tasa { get; set; }
        public decimal valor { get; set; }
        public int order_id { get; set; }
        public SiesaOrder order { get; set; }
    }
}
