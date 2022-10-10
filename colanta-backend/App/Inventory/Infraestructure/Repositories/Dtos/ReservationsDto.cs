namespace colanta_backend.App.Inventory.Infraestructure
{
    using System;
    using System.Collections.Generic;
    public class ReservationsDto
    {
        public List<ItemDto> items { get; set; }
        public  PagingDto paging{ get; set; }
    }

    public class ItemDto
    {
        public string LockId { get; set; }
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public string SalesChannel { get; set; }
        public DateTime ReservationDateUtc { get; set; }
        public DateTime MaximumConfirmationDateUtc { get; set; }
        public DateTime ConfirmedDateUtc { get; set; }
        public string Status { get; set; }
        public DateTime DateUtcAcknowledgedOnBalanceSystem { get; set; }
        public string InternalStatus { get; set; }
    }

    public class PagingDto
    {
        public int page { get; set; }
        public int perPage { get; set; }
        public int total { get; set; }
        public int pages { get; set; }
    }
}
