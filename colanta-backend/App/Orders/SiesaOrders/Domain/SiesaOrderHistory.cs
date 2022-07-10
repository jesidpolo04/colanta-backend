namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    using System;
    public class SiesaOrderHistory
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string vtex_id { get; set; }
        public string order_json { get; set; }
        public DateTime created_at { get; set; }
    }
}
