namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    public class SiesaOrder
    {
        public int id { get; set; }
        public bool finalizado { get; set; }
        public string siesa_id { get; set; }
        public string co { get; set; }
        public string fecha { get; set; }
        public string doc_tercero { get; set; }
        public string fecha_entrega { get; set; }
        public string referencia_vtex { get; set; }
        public string cond_pago { get; set; }
        public string notas { get; set; }
        public string direccion { get; set; }
        public string negocio { get; set; }
        public decimal total_pedido { get; set; }
        public decimal total_descuento { get; set; }
        public SiesaOrderDetail[] detalles { get; set; }
        public SiesaOrderDiscount[] descuentos { get; set; }
    }
}
