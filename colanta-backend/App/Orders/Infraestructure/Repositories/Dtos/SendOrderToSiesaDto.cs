namespace colanta_backend.App.Orders.Infraestructure
{
    public class SendOrderToSiesaDto
    {
        public SendOrderToSiesaHeaderDto Encabezado { get; set; }
        public SendOrderToSiesaDetailDto[] Detalles { get; set; }
        public SendOrderToSiesaDiscountDto[] Descuentos { get; set; }
    }

    public class SendOrderToSiesaHeaderDto
    {
        public string C263CO { get; set; }
        public string C263Fecha { get; set; }
        public string C263DocTercero { get; set; }
        public string C263FechaEntrega { get; set; }
        public string C263ReferenciaVTEX { get; set; }
        public string C263CondPago { get; set; }
        public string C263Notas { get; set; }
        public string C263Direccion { get; set; }
        public string C263Negocio { get; set; }
        public decimal C263TotalPedido { get; set; }
        public decimal C263TotalDescuentos { get; set; }
    }

    public class SendOrderToSiesaDetailDto
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

    public class SendOrderToSiesaDiscountDto
    {
        public string C263DestoCO {get; set; }
        public string C263ReferenciaVTEX {get; set; }
        public int C263NroDetalle {get; set; }
        public int C263OrdenDescto {get; set; }
        public decimal C263Tasa {get; set; }
        public decimal C263Valor { get; set; }
    }
}
