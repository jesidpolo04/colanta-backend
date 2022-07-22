﻿namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using SiesaOrders.Domain;
    using System.Collections.Generic;
    public class EFSiesaOrder
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public bool finalizado { get; set; }
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
        public string negocio { get; set; }
        public decimal total_pedido { get; set; }
        public decimal total_descuento { get; set; }
        public List<EFSiesaOrderDetail> detalles { get; set; }
        public List<EFSiesaOrderDiscount> descuentos { get; set; }

        public void setEfSiesaOrderFromSiesaOrder(SiesaOrder siesaOrder)
        {
            this.siesa_id = siesaOrder.siesa_id;
            this.co = siesaOrder.co;
            this.finalizado = siesaOrder.finalizado;
            this.fecha = siesaOrder.fecha;
            this.doc_tercero = siesaOrder.doc_tercero;
            this.fecha_entrega = siesaOrder.fecha_entrega;
            this.referencia_vtex = siesaOrder.referencia_vtex;
            this.estado_vtex = siesaOrder.estado_vtex;
            this.id_metodo_pago_vtex = siesaOrder.id_metodo_pago_vtex;
            this.metodo_pago_vtex = siesaOrder.metodo_pago_vtex;
            this.cond_pago = siesaOrder.cond_pago;
            this.notas = siesaOrder.notas;
            this.direccion = siesaOrder.direccion;
            this.negocio = siesaOrder.negocio;
            this.total_pedido = siesaOrder.total_pedido;
            this.total_descuento = siesaOrder.total_descuento;

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
        }

        public SiesaOrder getSiesaOrderFromEfSiesaOrder()
        {
            SiesaOrder siesaOrder = new SiesaOrder();

            siesaOrder.id = this.id;
            siesaOrder.siesa_id = this.siesa_id;
            siesaOrder.co = this.co;
            siesaOrder.finalizado = this.finalizado;
            siesaOrder.fecha = this.fecha;
            siesaOrder.doc_tercero = this.doc_tercero;
            siesaOrder.fecha_entrega = this.fecha_entrega;
            siesaOrder.referencia_vtex = this.referencia_vtex;
            siesaOrder.estado_vtex = this.estado_vtex;
            siesaOrder.id_metodo_pago_vtex = this.id_metodo_pago_vtex;
            siesaOrder.metodo_pago_vtex = this.metodo_pago_vtex;
            siesaOrder.cond_pago = this.cond_pago;
            siesaOrder.notas = this.notas;
            siesaOrder.direccion = this.direccion;
            siesaOrder.negocio = this.negocio;
            siesaOrder.total_pedido = this.total_pedido;
            siesaOrder.total_descuento = this.total_descuento;

            List<SiesaOrderDetail> siesaOrderDetails = new List<SiesaOrderDetail>();
            foreach (EFSiesaOrderDetail efSiesaOrderDetail in this.detalles)
            {
                siesaOrderDetails.Add(efSiesaOrderDetail.getSiesaOrderDetailFromEfSiesaOrderDetail());
            }
            siesaOrder.detalles = siesaOrderDetails.ToArray();

            List<SiesaOrderDiscount> siesaOrderDiscounts = new List<SiesaOrderDiscount>();
            foreach (EFSiesaOrderDiscount efSiesaOrderDiscount in this.descuentos)
            {
                siesaOrderDiscounts.Add(efSiesaOrderDiscount.getSiesaOrderDiscountFromEfSiesaOrderDiscount());
            }
            siesaOrder.descuentos = siesaOrderDiscounts.ToArray();

            return siesaOrder;
        }
    }
}