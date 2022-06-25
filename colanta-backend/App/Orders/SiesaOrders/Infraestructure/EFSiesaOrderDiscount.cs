namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using SiesaOrders.Domain;
    public class EFSiesaOrderDiscount
    {
        public int id { get; set; }
        public string desto_co { get; set; }
        public string referencia_vtex { get; set; }
        public int nro_detalle { get; set; }
        public int orden_descuento { get; set; }
        public decimal tasa { get; set; }
        public decimal valor { get; set; }
        public int order_id { get; set; }
        public EFSiesaOrder order { get; set; }

        public void setEfSiesaOrderDiscountFromSiesaOrderDiscount(SiesaOrderDiscount siesaOrderDiscount)
        {
            this.desto_co = siesaOrderDiscount.desto_co;
            this.referencia_vtex = siesaOrderDiscount.referencia_vtex;
            this.nro_detalle = siesaOrderDiscount.nro_detalle;
            this.orden_descuento = siesaOrderDiscount.orden_descuento;
            this.tasa = siesaOrderDiscount.tasa;
            this.valor = siesaOrderDiscount.valor;
        }

        public SiesaOrderDiscount getSiesaOrderDiscountFromEfSiesaOrderDiscount()
        {
            SiesaOrderDiscount siesaOrderDiscount = new SiesaOrderDiscount();
            siesaOrderDiscount.desto_co = this.desto_co;
            siesaOrderDiscount.referencia_vtex = this.referencia_vtex;
            siesaOrderDiscount.nro_detalle = this.nro_detalle;
            siesaOrderDiscount.orden_descuento = this.orden_descuento;
            siesaOrderDiscount.tasa = this.tasa;
            siesaOrderDiscount.valor = this.valor;
            siesaOrderDiscount.order_id = this.order_id;
            return siesaOrderDiscount;
        }
    }
}
