namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using SiesaOrders.Domain;
    public class EFSiesaOrderTax
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
        public EFSiesaOrder Order { get; set; }

        public void SetEfSiesaOrderTaxFromSiesaOrderTax(SiesaOrderTax siesaOrderTax)
        {
            NroDetalle = siesaOrderTax.NroDetalle;
            ReferenciaItem = siesaOrderTax.ReferenciaItem;
            PrecioBase = siesaOrderTax.PrecioBase;
            PrecioCompleto = siesaOrderTax.PrecioCompleto;
            IvaValor = siesaOrderTax.IvaValor;
            IvaPorcentaje = siesaOrderTax.IvaPorcentaje;
            ImpuestoConsumoValor = siesaOrderTax.ImpuestoConsumoValor;
            ImpuestoSaludableValor = siesaOrderTax.ImpuestoSaludableValor;
            ImpuestoSaludablePorcentaje = siesaOrderTax.ImpuestoSaludablePorcentaje;
            OrderId = siesaOrderTax.OrderId;
        }

        public SiesaOrderTax GetSiesaOrderTaxFromEfSiesaOrderTax()
        {
            return new SiesaOrderTax
            {
                NroDetalle = NroDetalle,
                ReferenciaItem = ReferenciaItem,
                PrecioBase = PrecioBase,
                PrecioCompleto = PrecioCompleto,
                IvaValor = IvaValor,
                IvaPorcentaje = IvaPorcentaje,
                ImpuestoConsumoValor = ImpuestoConsumoValor,
                ImpuestoSaludableValor = ImpuestoSaludableValor,
                ImpuestoSaludablePorcentaje = ImpuestoSaludablePorcentaje,
                OrderId = OrderId
            };
        }

    }
}
