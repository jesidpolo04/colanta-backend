namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using Orders.Domain;
    public class EFOrderHistory
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string json { get; set; }
        public DateTime created_at { get; set; }

        public void setEfOrderHistoryFromOrderHistory(OrderHistory orderHistory)
        {
            this.id = orderHistory.id;
            this.vtex_id = orderHistory.vtex_id;
            this.json = orderHistory.json;
        }

        public OrderHistory getOrderHistoryFromEfOrderHistory()
        {
            OrderHistory orderHistory = new OrderHistory();
            orderHistory.id = this.id;
            orderHistory.vtex_id = this.vtex_id;
            orderHistory.json = this.json;
            return orderHistory;
        }
    }
}
