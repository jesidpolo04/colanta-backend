namespace colanta_backend.App.Orders.Infraestructure
{
    using System.Collections.Generic;
    using Orders.SiesaOrders.Domain;
    using System.Text.Json;
    public class UpdatedSiesaOrderResponseDto
    {
        public bool finalizado { get; set; }
        public bool PedidoCancelado { get; set; }
        public SiesaOrderHeaderDto Encabezado { get; set; }
        public List<SiesaOrderDetailDto> Detalles { get; set; }
        public List<WayToPayDto> FormasPago { get; set; }
        public List <SiesaOrderDiscountDto> Descuentos { get; set; }

        public SiesaOrder getSiesaOrderFromDto()
        {
            SiesaOrder siesaOrder = new SiesaOrder
            {
                co = this.Encabezado.C263CO,
                fecha = this.Encabezado.C263Fecha,
                doc_tercero = this.Encabezado.C263DocTercero,
                fecha_entrega = this.Encabezado.C263FechaEntrega,
                referencia_vtex = this.Encabezado.C263ReferenciaVTEX,
                cond_pago = this.Encabezado.C263CondPago,
                notas = this.Encabezado.C263Notas,
                direccion = this.Encabezado.C263Direccion,
                departamento = this.Encabezado.C263Departamento,
                ciudad = this.Encabezado.C263Ciudad,
                negocio = this.Encabezado.C263Negocio,
                total_pedido = this.Encabezado.C263TotalPedido,
                total_envio = this.Encabezado.C263ValorEnvio,
                total_descuento = this.Encabezado.C263TotalDescuentos,
                recoge_en_tienda = this.Encabezado.C263RecogeEnTienda,
                formas_de_pago = JsonSerializer.Serialize(this.FormasPago),
                pago_contraentrega = this.Encabezado.C263PagoContraentrega,
                finalizado = this.finalizado,
                cancelado = this.PedidoCancelado
            };

            List<SiesaOrderDetail> siesaOrderDetails = new List<SiesaOrderDetail>();
            foreach (SiesaOrderDetailDto siesaOrderDetailDto in this.Detalles)
            {
                SiesaOrderDetail siesaOrderDetail = new SiesaOrderDetail
                {
                    det_co = siesaOrderDetailDto.C263DetCO,
                    nro_detalle = siesaOrderDetailDto.C263NroDetalle,
                    referencia_item = siesaOrderDetailDto.C263ReferenciaItem,
                    variacion_item = siesaOrderDetailDto.C263VariacionItem,
                    ind_obsequio = siesaOrderDetailDto.C263IndObsequio,
                    unidad_medida = siesaOrderDetailDto.C263UnidMedida,
                    cantidad = siesaOrderDetailDto.C263Cantidad,
                    precio = siesaOrderDetailDto.C263Precio,
                    notas = siesaOrderDetailDto.C263Notas,
                    impuesto = siesaOrderDetailDto.C263Impuesto,
                    referencia_vtex = siesaOrderDetailDto.C263ReferenciaVTEX
                };

                siesaOrderDetails.Add(siesaOrderDetail);
            }
            siesaOrder.detalles = siesaOrderDetails.ToArray();

            List<SiesaOrderDiscount> siesaOrderDiscounts = new List<SiesaOrderDiscount>();
            foreach (SiesaOrderDiscountDto siesaOrderDiscountDto in this.Descuentos)
            {
                SiesaOrderDiscount siesaOrderDiscount = new SiesaOrderDiscount
                {
                    desto_co = siesaOrderDiscountDto.C263DestoCO,
                    referencia_vtex = siesaOrderDiscountDto.C263ReferenciaVTEX,
                    nro_detalle = siesaOrderDiscountDto.C263NroDetalle,
                    orden_descuento = siesaOrderDiscountDto.C263OrdenDescto,
                    tasa = siesaOrderDiscountDto.C263Tasa,
                    valor = siesaOrderDiscountDto.C263Valor
                };

                siesaOrderDiscounts.Add(siesaOrderDiscount);
            }
            siesaOrder.descuentos = siesaOrderDiscounts.ToArray();

            List<SiesaOrderTax> siesaOrderTaxes = new List<SiesaOrderTax>();
            siesaOrder.impuestos = siesaOrderTaxes.ToArray();
            return siesaOrder;
        }
    }

}
