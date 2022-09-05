namespace colanta_backend.App.Orders.Infraestructure
{
    using System.Collections.Generic;
    using Orders.SiesaOrders.Domain;
    using System.Text.Json;
    public class UpdatedSiesaOrderResponseDto
    {
        public bool finalizado { get; set; }
        public SiesaOrderHeaderDto encabezado { get; set; }
        public List<SiesaOrderDetailDto> detalles { get; set; }
        public List<WayToPayDto> formasPago { get; set; }
        public List <SiesaOrderDiscountDto> descuentos { get; set; }

        public SiesaOrder getSiesaOrderFromDto()
        {
            SiesaOrder siesaOrder = new SiesaOrder();
            siesaOrder.co = this.encabezado.C263CO;
            siesaOrder.fecha = this.encabezado.C263Fecha;
            siesaOrder.doc_tercero = this.encabezado.C263DocTercero;
            siesaOrder.fecha_entrega = this.encabezado.C263FechaEntrega;
            siesaOrder.referencia_vtex = this.encabezado.C263ReferenciaVTEX;
            siesaOrder.cond_pago = this.encabezado.C263CondPago;
            siesaOrder.notas = this.encabezado.C263Notas;
            siesaOrder.direccion = this.encabezado.C263Direccion;
            siesaOrder.departamento = this.encabezado.C263Departamento;
            siesaOrder.ciudad = this.encabezado.C263Ciudad;
            siesaOrder.negocio = this.encabezado.C263Negocio;
            siesaOrder.total_pedido = this.encabezado.C263TotalPedido;
            siesaOrder.total_envio = this.encabezado.C263ValorEnvio;
            siesaOrder.total_descuento = this.encabezado.C263TotalDescuentos;
            siesaOrder.recoge_en_tienda = this.encabezado.C263RecogeEnTienda;
            siesaOrder.formas_de_pago = JsonSerializer.Serialize(this.formasPago);
            siesaOrder.pago_contraentrega = this.encabezado.C263PagoContraentrega;
            siesaOrder.finalizado = this.finalizado;

            List<SiesaOrderDetail> siesaOrderDetails = new List<SiesaOrderDetail>();
            foreach (SiesaOrderDetailDto siesaOrderDetailDto in this.detalles)
            {
                SiesaOrderDetail siesaOrderDetail = new SiesaOrderDetail();
                siesaOrderDetail.det_co = siesaOrderDetailDto.C263DetCO;
                siesaOrderDetail.nro_detalle = siesaOrderDetailDto.C263NroDetalle;
                siesaOrderDetail.referencia_item = siesaOrderDetailDto.C263ReferenciaItem;
                siesaOrderDetail.variacion_item = siesaOrderDetailDto.C263VariacionItem;
                siesaOrderDetail.ind_obsequio = siesaOrderDetailDto.C263IndObsequio;
                siesaOrderDetail.unidad_medida = siesaOrderDetailDto.C263UnidMedida;
                siesaOrderDetail.cantidad = siesaOrderDetailDto.C263Cantidad;
                siesaOrderDetail.precio = siesaOrderDetailDto.C263Precio;
                siesaOrderDetail.notas = siesaOrderDetailDto.C263Notas;
                siesaOrderDetail.impuesto = siesaOrderDetailDto.C263Impuesto;
                siesaOrderDetail.referencia_vtex = siesaOrderDetailDto.C263ReferenciaVTEX;

                siesaOrderDetails.Add(siesaOrderDetail);
            }
            siesaOrder.detalles = siesaOrderDetails.ToArray();

            List<SiesaOrderDiscount> siesaOrderDiscounts = new List<SiesaOrderDiscount>();
            foreach (SiesaOrderDiscountDto siesaOrderDiscountDto in this.descuentos)
            {
                SiesaOrderDiscount siesaOrderDiscount = new SiesaOrderDiscount();
                siesaOrderDiscount.desto_co = siesaOrderDiscountDto.C263DestoCO;
                siesaOrderDiscount.referencia_vtex = siesaOrderDiscountDto.C263ReferenciaVTEX;
                siesaOrderDiscount.nro_detalle = siesaOrderDiscountDto.C263NroDetalle;
                siesaOrderDiscount.orden_descuento = siesaOrderDiscountDto.C263OrdenDescto;
                siesaOrderDiscount.tasa = siesaOrderDiscountDto.C263Tasa;
                siesaOrderDiscount.valor = siesaOrderDiscountDto.C263Valor;

                siesaOrderDiscounts.Add(siesaOrderDiscount);
            }
            siesaOrder.descuentos = siesaOrderDiscounts.ToArray();
            return siesaOrder;
        }
    }

}
}
