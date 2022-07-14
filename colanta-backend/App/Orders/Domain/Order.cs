namespace colanta_backend.App.Orders.Domain
{
    using System;
    public class Order
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string status { get; set; }
        public string last_status { get; set; }
        public string order_json { get; set; }
        public string last_change_date { get; set; }
        public string current_change_date { get; set; }
        public string business { get; set; }

    }
}
