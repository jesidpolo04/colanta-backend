namespace colanta_backend.App.Orders.Controllers
{
    using Orders.Domain;
    public class OrderHookDto
    {
        public string Domain { get; set; }
        public string OrderId { get; set; }
        public string State { get; set; }
        public string LastState { get; set; }
        public string LastChange { get; set; }
        public string CurrentChange { get; set; }
        public OrderHookOriginDto Origin { get; set; }

        public Order getOrderFromDto() {
            Order order = new Order();

            order.vtex_id = OrderId;
            order.last_status = LastState;
            order.status = State;
            order.current_change_date = CurrentChange;
            order.last_change_date = LastChange;
            order.business = "";
            return order;
        }
    }

    public class OrderHookOriginDto
    {
        public string Account { get; set; }
        public string Key { get; set; }
    }
}
