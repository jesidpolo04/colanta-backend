using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    using Inventory.Domain;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    public class NewOrderMailModel : PageModel
    {
        public SiesaOrder siesaOrder;
        public Warehouse store;
        public string storeName;
        public string vtexOrderId;
        public string siesaPedido;
        public DateTime deliveryDate;
        public DateTime orderDate;
        public bool pickupInStore;
        public List<WayToPay> wayToPays;
        public NewOrderMailModel(SiesaOrder siesaOrder, Warehouse store)
        {
            this.store = store;
            this.storeName = store.name;
            this.siesaOrder = siesaOrder;
            this.vtexOrderId = siesaOrder.referencia_vtex;
            this.siesaPedido = siesaOrder.siesa_pedido;
            this.deliveryDate = DateTime.Parse(siesaOrder.fecha_entrega);
            this.orderDate = DateTime.Parse(siesaOrder.fecha);
            this.pickupInStore = siesaOrder.recoge_en_tienda;
            this.wayToPays = JsonSerializer.Deserialize<List<WayToPay>>(siesaOrder.formas_de_pago);
        }
        public void OnGet()
        {
        }
    }
}
