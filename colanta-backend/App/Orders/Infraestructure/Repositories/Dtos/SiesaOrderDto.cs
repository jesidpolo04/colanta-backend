namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    using System.Collections.Generic;

    public class SiesaOrderDto
    {
        public SiesaOrderHeaderDto Encabezado { get; set; }
        public SiesaOrderDetailDto[] Detalles { get; set; }
        public SiesaOrderDiscountDto[] Descuentos { get; set; }

        public SiesaOrder getSiesaOrderFromDto()
        {
            SiesaOrder siesaOrder = new SiesaOrder();
            siesaOrder.co = this.Encabezado.C263CO;
            siesaOrder.fecha = this.Encabezado.C263Fecha;
            siesaOrder.doc_tercero = this.Encabezado.C263DocTercero;
            siesaOrder.fecha_entrega = this.Encabezado.C263FechaEntrega;
            siesaOrder.referencia_vtex = this.Encabezado.C263ReferenciaVTEX;
            siesaOrder.cond_pago = this.Encabezado.C263CondPago;
            siesaOrder.notas = this.Encabezado.C263Notas;
            siesaOrder.direccion = this.Encabezado.C263Direccion;
            siesaOrder.departamento = this.Encabezado.C263Departamento;
            siesaOrder.ciudad = this.Encabezado.C263Ciudad;
            siesaOrder.negocio = this.Encabezado.C263Negocio;
            siesaOrder.total_pedido = this.Encabezado.C263TotalPedido;
            siesaOrder.total_descuento = this.Encabezado.C263TotalDescuentos;
            siesaOrder.recoge_en_tienda = this.Encabezado.C263RecogeEnTienda;
            
            List<SiesaOrderDetail> siesaOrderDetails = new List<SiesaOrderDetail>();
            foreach(SiesaOrderDetailDto siesaOrderDetailDto in this.Detalles)
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
            foreach (SiesaOrderDiscountDto siesaOrderDiscountDto in this.Descuentos)
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

    public class SiesaOrderHeaderDto
    {
        public string C263CO { get; set; }
        public string C263Fecha { get; set; }
        public string C263DocTercero { get; set; }
        public string C263FechaEntrega { get; set; }
        public string C263ReferenciaVTEX { get; set; }
        public string C263ReferenciaPago { get; set; }
        public string C263CondPago { get; set; }
        public string C263Notas { get; set; }
        public string C263Direccion { get; set; }
        public string C263Ciudad { get; set; }
        public string C263Departamento { get; set; }
        public string C263Negocio { get; set; }
        public decimal C263TotalPedido { get; set; }
        public decimal C263TotalDescuentos { get; set; }
        public bool C263RecogeEnTienda { get; set; }
    }

    public class SiesaOrderDetailDto
    {
        public string C263DetCO {get; set;}
        public int C263NroDetalle {get; set;}
        public string C263ReferenciaItem {get; set;}
        public string? C263VariacionItem {get; set;}
        public int C263IndObsequio {get; set;}
        public string C263UnidMedida {get; set;}
        public int C263Cantidad {get; set;}
        public decimal C263Precio {get; set;}
        public string C263Notas {get; set;}
        public decimal C263Impuesto {get; set;}
        public string C263ReferenciaVTEX { get; set; }
    }

    public class SiesaOrderDiscountDto
    {
        public string C263DestoCO {get; set; }
        public string C263ReferenciaVTEX {get; set; }
        public string C263ReferenciaDescuento { get; set; }
        public int C263NroDetalle {get; set; }
        public int C263OrdenDescto {get; set; }
        public decimal C263Tasa {get; set; }
        public decimal C263Valor { get; set; }
    }
}
