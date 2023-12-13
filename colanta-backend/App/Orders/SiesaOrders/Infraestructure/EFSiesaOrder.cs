namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using SiesaOrders.Domain;
    using System;
    using System.Collections.Generic;
    public class EFSiesaOrder
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public bool finalizado { get; set; }
        public bool cancelado { get; set; }
        public string co { get; set; }
        public string fecha { get; set; }
        public string doc_tercero { get; set; }
        public string fecha_entrega { get; set; }
        public string referencia_vtex { get; set; }
        public string estado_vtex { get; set; }
        public string id_metodo_pago_vtex { get; set; }
        public string metodo_pago_vtex { get; set; }
        public string cond_pago { get; set; }
        public string notas { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string negocio { get; set; }
        public decimal total_envio { get; set; }
        public string formas_de_pago { get; set; }
        public decimal total_pedido { get; set; }
        public decimal total_descuento { get; set; }
        public bool recoge_en_tienda { get; set; }
        public DateTime? fecha_recoge { get; set; }
        public bool pago_contraentrega { get; set; }
        public string telefono { get; set; }
        public List<EFSiesaOrderDetail> detalles { get; set; }
        public List<EFSiesaOrderDiscount> descuentos { get; set; }
        public List<EFSiesaOrderTax> impuestos { get; set; }

        public void setEfSiesaOrderFromSiesaOrder(SiesaOrder siesaOrder)
        {
            siesa_id = siesaOrder.siesa_id;
            co = siesaOrder.co;
            finalizado = siesaOrder.finalizado;
            cancelado = siesaOrder.cancelado;
            fecha = siesaOrder.fecha;
            doc_tercero = siesaOrder.doc_tercero;
            fecha_entrega = siesaOrder.fecha_entrega;
            referencia_vtex = siesaOrder.referencia_vtex;
            estado_vtex = siesaOrder.estado_vtex;
            id_metodo_pago_vtex = siesaOrder.id_metodo_pago_vtex;
            metodo_pago_vtex = siesaOrder.metodo_pago_vtex;
            cond_pago = siesaOrder.cond_pago;
            notas = siesaOrder.notas;
            direccion = siesaOrder.direccion;
            departamento = siesaOrder.departamento;
            ciudad = siesaOrder.ciudad;
            negocio = siesaOrder.negocio;
            total_pedido = siesaOrder.total_pedido;
            total_descuento = siesaOrder.total_descuento;
            total_envio = siesaOrder.total_envio;
            formas_de_pago = siesaOrder.formas_de_pago;
            pago_contraentrega = siesaOrder.pago_contraentrega;
            recoge_en_tienda = siesaOrder.recoge_en_tienda;
            fecha_recoge = siesaOrder.fecha_recoge;
            telefono = siesaOrder.telefono;


            List<EFSiesaOrderDetail> efSiesaOrderDetails = new List<EFSiesaOrderDetail>();
            foreach(SiesaOrderDetail siesaOrderDetail in siesaOrder.detalles)
            {
                EFSiesaOrderDetail efSiesaOrderDetail = new EFSiesaOrderDetail();
                efSiesaOrderDetail.setEfSiesaOrderDetailFromSiesaOrderDetail(siesaOrderDetail);
                efSiesaOrderDetails.Add(efSiesaOrderDetail);
            }
            this.detalles = efSiesaOrderDetails;

            List<EFSiesaOrderDiscount> efSiesaOrderDiscounts = new List<EFSiesaOrderDiscount>();
            foreach(SiesaOrderDiscount siesaOrderDiscount in siesaOrder.descuentos)
            {
                EFSiesaOrderDiscount efSiesaOrderDiscount = new EFSiesaOrderDiscount();
                efSiesaOrderDiscount.setEfSiesaOrderDiscountFromSiesaOrderDiscount(siesaOrderDiscount);
                efSiesaOrderDiscounts.Add(efSiesaOrderDiscount);
            }
            this.descuentos = efSiesaOrderDiscounts;

            List<EFSiesaOrderTax> efSiesaOrderTaxes = new List<EFSiesaOrderTax>();
            foreach(SiesaOrderTax siesaOrderTax in siesaOrder.impuestos)
            {
                EFSiesaOrderTax efSiesaOrderTax = new EFSiesaOrderTax();
                efSiesaOrderTax.SetEfSiesaOrderTaxFromSiesaOrderTax(siesaOrderTax);
                efSiesaOrderTaxes.Add(efSiesaOrderTax);
            }
            this.impuestos = efSiesaOrderTaxes;
        }

        public SiesaOrder getSiesaOrderFromEfSiesaOrder()
        {
            SiesaOrder siesaOrder = new SiesaOrder
            {
                id = this.id,
                siesa_id = this.siesa_id,
                co = this.co,
                finalizado = this.finalizado,
                cancelado = this.cancelado,
                fecha = this.fecha,
                doc_tercero = this.doc_tercero,
                fecha_entrega = this.fecha_entrega,
                referencia_vtex = this.referencia_vtex,
                estado_vtex = this.estado_vtex,
                id_metodo_pago_vtex = this.id_metodo_pago_vtex,
                metodo_pago_vtex = this.metodo_pago_vtex,
                cond_pago = this.cond_pago,
                notas = this.notas,
                direccion = this.direccion,
                departamento = this.departamento,
                ciudad = this.ciudad,
                recoge_en_tienda = this.recoge_en_tienda,
                fecha_recoge = this.fecha_recoge,
                negocio = this.negocio,
                total_pedido = this.total_pedido,
                total_descuento = this.total_descuento,
                total_envio = this.total_envio,
                pago_contraentrega = this.pago_contraentrega,
                formas_de_pago = this.formas_de_pago,
                telefono = this.telefono
            };

            List<SiesaOrderDetail> siesaOrderDetails = new List<SiesaOrderDetail>();
            foreach (EFSiesaOrderDetail efSiesaOrderDetail in detalles)
            {
                siesaOrderDetails.Add(efSiesaOrderDetail.getSiesaOrderDetailFromEfSiesaOrderDetail());
            }
            siesaOrder.detalles = siesaOrderDetails.ToArray();

            List<SiesaOrderDiscount> siesaOrderDiscounts = new List<SiesaOrderDiscount>();
            foreach (EFSiesaOrderDiscount efSiesaOrderDiscount in descuentos)
            {
                siesaOrderDiscounts.Add(efSiesaOrderDiscount.getSiesaOrderDiscountFromEfSiesaOrderDiscount());
            }
            siesaOrder.descuentos = siesaOrderDiscounts.ToArray();

            List<SiesaOrderTax> siesaOrderTaxes = new List<SiesaOrderTax>();
            foreach(EFSiesaOrderTax efSiesaOrderTax in impuestos){
                siesaOrderTaxes.Add(efSiesaOrderTax.GetSiesaOrderTaxFromEfSiesaOrderTax());
            }
            siesaOrder.impuestos = siesaOrderTaxes.ToArray();

            return siesaOrder;
        }
    }
}
