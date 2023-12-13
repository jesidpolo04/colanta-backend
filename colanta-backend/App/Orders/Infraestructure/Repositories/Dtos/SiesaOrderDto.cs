namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Shared.Infraestructure.Converters;
    using System;
    using System.Globalization;
    using colanta_backend.App.Shared.Domain;

    public class SiesaOrderDto
    {
        public SiesaOrderHeaderDto Encabezado { get; set; }
        public List<SiesaOrderDetailDto> Detalles { get; set; }
        public List<SiesaOrderDiscountDto> Descuentos { get; set; }
        public List<SiesaOrderTaxDto> ImpuestosPedido { get; set; }
        public List<WayToPayDto> FormasPago { get; set; }


        public SiesaOrderDto()
        {
            Encabezado = new SiesaOrderHeaderDto();
            Detalles = new List<SiesaOrderDetailDto>();
            Descuentos = new List<SiesaOrderDiscountDto>();
            ImpuestosPedido = new List<SiesaOrderTaxDto>();
            FormasPago = new List<WayToPayDto>();
        }

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
                fecha_recoge = this.Encabezado.C263FechaRecoge != null ? DateTime.ParseExact(this.Encabezado.C263FechaRecoge, DateFormats.FECHA_RECOGE, CultureInfo.InvariantCulture) : null,
                formas_de_pago = JsonSerializer.Serialize(this.FormasPago),
                pago_contraentrega = this.Encabezado.C263PagoContraentrega
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
            foreach (SiesaOrderTaxDto siesaOrderTaxDto in ImpuestosPedido)
            {
                SiesaOrderTax siesaOrderTax = new SiesaOrderTax
                {
                    NroDetalle = siesaOrderTaxDto.C263NroDetalle,
                    IvaValor = siesaOrderTaxDto.C263IvaValor,
                    IvaPorcentaje = siesaOrderTaxDto.C263IvaPorcen,
                    ImpuestoConsumoValor = siesaOrderTaxDto.C263IpoConsumoValor,
                    ImpuestoSaludableValor = siesaOrderTaxDto.C263SaludableValor,
                    ImpuestoSaludablePorcentaje = siesaOrderTaxDto.C263SaludablePorcen,
                    PrecioBase = siesaOrderTaxDto.C263PrecioBase,
                    PrecioCompleto = siesaOrderTaxDto.C263PrecioCompleto,
                    ReferenciaItem = siesaOrderTaxDto.C263ReferenciaItem
                };
                siesaOrderTaxes.Add(siesaOrderTax);;
            }
            siesaOrder.impuestos = siesaOrderTaxes.ToArray();

            return siesaOrder;
        }
    }

    public class SiesaOrderHeaderDto
    {
        public string C263CO { get; set; }
        public string C263Fecha { get; set; }
        public string C263DocTercero { get; set; }
        public string C263Telefono { get; set; }
        public string C263FechaEntrega { get; set; }
        public string C263ReferenciaVTEX { get; set; }
        public string C263ReferenciaPago { get; set; }
        public bool C263PagoContraentrega { get; set; }
        public decimal C263ValorEnvio { get; set; }
        public string C263CondPago { get; set; }
        public string C263Notas { get; set; }
        public string C263Direccion { get; set; }
        public string C263Nombres { get; set; }
        public string C263Ciudad { get; set; }
        public string C263Departamento { get; set; }
        public string C263Negocio { get; set; }
        public decimal C263TotalPedido { get; set; }
        public decimal C263TotalDescuentos { get; set; }
        public bool C263RecogeEnTienda { get; set; }
        public string? C263FechaRecoge { get; set; }
    }

    public class WayToPayDto
    {
        public string C263FormaPago { get; set; }
        public string C263ReferenciaPago { get; set; }
        public decimal C263Valor { get; set; }
    }

    public class SiesaOrderDetailDto
    {
        public string C263DetCO { get; set; }
        public int C263NroDetalle { get; set; }
        public string C263ReferenciaItem { get; set; }
        public string? C263VariacionItem { get; set; }
        public int C263IndObsequio { get; set; }
        public string C263UnidMedida { get; set; }
        public decimal C263Cantidad { get; set; }
        public decimal C263Precio { get; set; }
        public string C263Notas { get; set; }
        public decimal C263Impuesto { get; set; }
        public string C263ReferenciaVTEX { get; set; }
    }

    public class SiesaOrderDiscountDto
    {
        public string C263DestoCO { get; set; }
        public string C263ReferenciaVTEX { get; set; }
        public string C263ReferenciaDescuento { get; set; }
        public int C263NroDetalle { get; set; }
        public int C263OrdenDescto { get; set; }
        public decimal C263Tasa { get; set; }
        public decimal C263Valor { get; set; }
    }

    public class SiesaOrderTaxDto
    {
        public int C263NroDetalle { get; set; }
        public int C263CodPedido { get; set; }
        public string C263ReferenciaItem { get; set; }
        public decimal C263PrecioBase { get; set; }
        public decimal C263PrecioCompleto { get; set; }
        public decimal C263IvaValor { get; set; }
        public decimal C263IvaPorcen { get; set; }
        public decimal C263IpoConsumoValor { get; set; }
        public decimal C263SaludableValor { get; set; }
        public decimal C263SaludablePorcenValor { get; set; }
        public decimal C263SaludablePorcen { get; set; }
    }
}
