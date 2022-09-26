using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.SiesaOrders.Domain;
    using Orders.Domain;
    using Inventory.Domain;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    public class NewOrderMailModel : PageModel
    {
        public SiesaOrder siesaOrder;
        public VtexOrder vtexOrder;
        public Warehouse store;
        public string storeName;
        public string vtexOrderId;
        public string siesaPedido; //campo que envía cristian en conjunto con el id del pedido
        public DateTime deliveryDate;
        public DateTime orderDate;
        public bool pickupInStore;
        public List<WayToPay> wayToPays;
        public DateTime? pickupStart;
        public DateTime? pickupEnd;
        public string observations;

        public NewOrderMailModel(SiesaOrder siesaOrder, VtexOrder vtexOrder, Warehouse store)
        {
            DeliveryWindow? deliveryWindow = vtexOrder.shippingData.logisticsInfo[0].deliveryWindow;
            this.store = store;
            this.storeName = store.name;
            this.vtexOrder = vtexOrder;
            this.siesaOrder = siesaOrder;
            this.vtexOrderId = siesaOrder.referencia_vtex;
            this.siesaPedido = siesaOrder.siesa_pedido;
            this.deliveryDate = DateTime.Parse(siesaOrder.fecha_entrega);
            this.orderDate = DateTime.Parse(siesaOrder.fecha);
            this.pickupInStore = siesaOrder.recoge_en_tienda;
            this.wayToPays = JsonSerializer.Deserialize<List<WayToPay>>(siesaOrder.formas_de_pago);
            this.pickupStart = deliveryWindow != null ? deliveryWindow.startDateUtc?.ToUniversalTime() : null;
            this.pickupEnd = deliveryWindow != null ? deliveryWindow.endDateUtc?.ToUniversalTime() : null;
            this.observations = this.siesaOrder.notas;
        }
        public void OnGet()
        {
        }
    }
}
