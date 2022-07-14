namespace colanta_backend.App.Orders.Domain
{
    using System;
    public class OrderHistory
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string json { get; set; }
        public DateTime created_at { get; set; }
    }
}
