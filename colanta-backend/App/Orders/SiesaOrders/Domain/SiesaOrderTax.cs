namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    public class SiesaOrderTax
    {
        public int Id { get; set; }

        public int NroDetalle { get; set; }
        public string ReferenciaItem { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioCompleto { get; set; }
        public decimal IvaValor { get; set; }
        public decimal IvaPorcentaje { get; set; }
        public decimal ImpuestoConsumoValor { get; set; }
        public decimal ImpuestoSaludableValor { get; set; }
        public decimal ImpuestoSaludablePorcentaje { get; set; }
        public int OrderId { get; set; }
        public SiesaOrder Order { get; set; }
    }
}
