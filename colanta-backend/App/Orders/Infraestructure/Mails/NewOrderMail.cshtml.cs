using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    public class NewOrderMailModel : PageModel
    {
        public SiesaOrder siesaOrder;
        public string vtexOrderId;
        public string siesaPedido;
        public DateTime deliveryDate;
        public bool isUponDelivery;
        public List<WayToPay> wayToPays;
        public NewOrderMailModel(SiesaOrder siesaOrder)
        {
            this.siesaOrder = siesaOrder;
            this.vtexOrderId = siesaOrder.referencia_vtex;
            this.siesaPedido = siesaOrder.siesa_pedido;
            this.deliveryDate = DateTime.Parse(siesaOrder.fecha_entrega);
            this.isUponDelivery = siesaOrder.recoge_en_tienda;
            this.wayToPays = JsonSerializer.Deserialize<List<WayToPay>>(siesaOrder.formas_de_pago);
        }
        public void OnGet()
        {
        }
    }
}
