namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.Domain;
    public class EFOrder
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string status { get; set; }
        public string last_status { get; set; }
        public string order_json { get; set; }
        public string last_change_date { get; set; }
        public string current_change_date { get; set; }
        public string business { get; set; }

        public void setEfOrderFromOrder(Order order)
        {
            this.id = order.id;
            this.vtex_id = order.vtex_id;
            this.status = order.status;
            this.last_status = order.last_status;
            this.order_json = order.order_json;
            this.last_change_date = order.last_change_date;
            this.current_change_date = order.current_change_date;
            this.business = order.business;

        }

        public Order getOrderFromEfOrder()
        {
            Order order = new Order();
            order.id = this.id;
            order.vtex_id = this.vtex_id;
            order.status = this.status;
            order.last_status = this.last_status;
            order.order_json = this.order_json;
            order.last_change_date = this.last_change_date;
            order.current_change_date = this.current_change_date;
            order.business = this.business;

            return order;
        }
    }
}
