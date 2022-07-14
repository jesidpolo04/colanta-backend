namespace colanta_backend.App.Orders.Infraestructure
{
    using System.Collections.Generic;
    using System;
    public class UpdateVtexOrderDto
    {
        public string requestId { get; set; }
        public string reason { get; set; }
        public int discountValue { get; set; }
        public int incrementValue { get; set; }
        public List<UpdateVtexOrderItemAddedDto> itemsAdded { get; set; }
        public List<UpdateVtexOrderItemRemovedDto> itemsRemoved { get; set; }

        public UpdateVtexOrderDto()
        {
            itemsAdded = new List<UpdateVtexOrderItemAddedDto>();
            itemsRemoved = new List<UpdateVtexOrderItemRemovedDto>();
        }

        public void addItem(string vtexId, decimal price, int quantity)
        {
            UpdateVtexOrderItemAddedDto itemAdded = new UpdateVtexOrderItemAddedDto();
            itemAdded.id = vtexId;
            itemAdded.price = Decimal.ToInt32(price * 100);
            itemAdded.quantity = quantity;
            itemsAdded.Add(itemAdded);
        }
        public void removeItem(string vtexId, decimal price, int quantity)
        {
            UpdateVtexOrderItemRemovedDto itemRemoved = new UpdateVtexOrderItemRemovedDto();
            itemRemoved.id = vtexId;
            itemRemoved.price = Decimal.ToInt32(price * 100);
            itemRemoved.quantity = quantity;
            itemsRemoved.Add(itemRemoved);
        }

        public void generateRequestId(string vtexOrderId)
        {
            this.requestId = vtexOrderId + "_" + DateTime.Now.DayOfYear;
        }

        public class UpdateVtexOrderItemAddedDto
        {
            public string id { get; set; }
            public int price { get; set; }
            public int quantity { get; set; }
        }

        public class UpdateVtexOrderItemRemovedDto
        {
            public string id { get; set; }
            public int price { get; set; }
            public int quantity { get; set; }
        }
    }
}