namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using SiesaOrders.Domain;
    public class EFSiesaOrderDetail
    {
        public int id { get; set; }
        public string det_co { get; set; }
        public int nro_detalle { get; set; }
        public string referencia_item { get; set; }
        public string variacion_item { get; set; }
        public int ind_obsequio { get; set; }
        public string unidad_medida { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public string notas { get; set; }
        public decimal impuesto { get; set; }
        public string referencia_vtex { get; set; }
        public int order_id { get; set; }
        public EFSiesaOrder order { get; set; }

        public void setEfSiesaOrderDetailFromSiesaOrderDetail(SiesaOrderDetail siesaOrderDetail)
        {
            this.det_co = siesaOrderDetail.det_co;
            this.nro_detalle = siesaOrderDetail.nro_detalle;
            this.referencia_item = siesaOrderDetail.referencia_item;
            this.variacion_item = siesaOrderDetail.variacion_item;
            this.ind_obsequio = siesaOrderDetail.ind_obsequio;
            this.unidad_medida = siesaOrderDetail.unidad_medida;
            this.cantidad = siesaOrderDetail.cantidad;
            this.precio = siesaOrderDetail.precio;
            this.notas = siesaOrderDetail.notas;
            this.impuesto = siesaOrderDetail.impuesto;
            this.referencia_vtex = siesaOrderDetail.referencia_vtex;
        }

        public SiesaOrderDetail getSiesaOrderDetailFromEfSiesaOrderDetail()
        {
            SiesaOrderDetail siesaOrderDetail = new SiesaOrderDetail();

            siesaOrderDetail.det_co = this.det_co ;
            siesaOrderDetail.nro_detalle = this.nro_detalle ;
            siesaOrderDetail.referencia_item = this.referencia_item ;
            siesaOrderDetail.variacion_item = this.variacion_item ;
            siesaOrderDetail.ind_obsequio = this.ind_obsequio ;
            siesaOrderDetail.unidad_medida = this.unidad_medida ;
            siesaOrderDetail.cantidad = this.cantidad ;
            siesaOrderDetail.precio = this.precio ;
            siesaOrderDetail.notas = this.notas ;
            siesaOrderDetail.impuesto = this.impuesto ;
            siesaOrderDetail.referencia_vtex = this.referencia_vtex;
            siesaOrderDetail.order_id = this.order_id;

            return siesaOrderDetail;
        }
    }
}
