namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    using System.Collections.Generic;
    public class SiesaOrder
    {
        public int id { get; set; }
        public bool finalizado { get; set; }
        public bool cancelado { get; set; }
        public string siesa_id { get; set; }
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
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string direccion { get; set; }
        public string negocio { get; set; }
        public decimal total_pedido { get; set; }
        public decimal total_descuento { get; set; }
        public decimal total_envio { get; set; }
        public string formas_de_pago { get; set; }
        public bool pago_contraentrega { get; set; }
        public bool recoge_en_tienda { get; set; }
        public string siesa_pedido { get; set; }
        public string telefono { get; set; }
        public SiesaOrderDetail[] detalles { get; set; }
        public SiesaOrderDiscount[] descuentos { get; set; }

        public List<AddedItem> getAddedItems(SiesaOrderDetail[] newOrderDetails)
        {
            List<AddedItem> addedItems = new List<AddedItem>();
            foreach(SiesaOrderDetail newDetail in newOrderDetails)
            {
                if (!this.itemExists(newDetail))
                {
                    AddedItem addedItem = new AddedItem();
                    addedItem.price = newDetail.precio;
                    addedItem.quantity = newDetail.cantidad;
                    addedItem.vtexId = newDetail.referencia_vtex;
                    addedItem.siesaId = newDetail.referencia_item;
                    addedItems.Add(addedItem);
                    continue;
                }

                foreach (SiesaOrderDetail detail in this.detalles)
                {
                    if (
                        newDetail.referencia_item == detail.referencia_item &&
                        newDetail.cantidad > detail.cantidad
                        ) 
                    {
                        AddedItem addedItem = new AddedItem();
                        addedItem.price = newDetail.precio;
                        addedItem.quantity = newDetail.cantidad - detail.cantidad;
                        addedItem.vtexId = newDetail.referencia_vtex;
                        addedItem.siesaId = newDetail.referencia_item;
                        addedItems.Add(addedItem);
                        continue;
                    }
                    if (
                        newDetail.referencia_item == detail.referencia_item &&
                        newDetail.cantidad == detail.cantidad &&
                        newDetail.precio != detail.precio
                        )
                    {
                        AddedItem addedItem = new AddedItem();
                        addedItem.price = newDetail.precio;
                        addedItem.quantity = newDetail.cantidad;
                        addedItem.vtexId = newDetail.referencia_vtex;
                        addedItem.siesaId = newDetail.referencia_item;
                        addedItems.Add(addedItem);
                        continue;
                    }
                }
            }
            return addedItems;
        }

        public List<RemovedItem> getRemovedItems(SiesaOrderDetail[] newOrderDetails)
        {
            List<RemovedItem> removedItems = new List<RemovedItem>();
            foreach (SiesaOrderDetail detail in this.detalles)
            {
                if (!detail.thisItemExists(newOrderDetails))
                {
                    RemovedItem removedItem = new RemovedItem();
                    removedItem.price = detail.precio;
                    removedItem.quantity = detail.cantidad;
                    removedItem.vtexId = detail.referencia_vtex;
                    removedItem.siesaId = detail.referencia_item;
                    removedItems.Add(removedItem);
                    continue;
                }
                foreach(SiesaOrderDetail newDetail in newOrderDetails)
                {
                    if (
                        newDetail.referencia_item == detail.referencia_item &&
                        newDetail.cantidad < detail.cantidad
                        )
                    {
                        RemovedItem removedItem = new RemovedItem();
                        removedItem.price = newDetail.precio;
                        removedItem.quantity = detail.cantidad - newDetail.cantidad;
                        removedItem.vtexId = detail.referencia_vtex;
                        removedItem.siesaId = detail.referencia_item;
                        removedItems.Add(removedItem);
                        continue;
                    }
                    if(
                        newDetail.referencia_item == detail.referencia_item &&
                        newDetail.cantidad == detail.cantidad &&
                        newDetail.precio != detail.precio
                        )
                    {
                        RemovedItem removedItem = new RemovedItem();
                        removedItem.price = detail.precio;
                        removedItem.quantity = detail.cantidad;
                        removedItem.vtexId = detail.referencia_vtex;
                        removedItem.siesaId = detail.referencia_item;
                        removedItems.Add(removedItem);
                        continue;
                    }
                }
            }
            return removedItems;
        }

        private bool itemExists(SiesaOrderDetail detail)
        {
            bool exists = false;
            foreach (SiesaOrderDetail thisDetail in this.detalles)
            {
                if (thisDetail.referencia_item == detail.referencia_item)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

    }
}
