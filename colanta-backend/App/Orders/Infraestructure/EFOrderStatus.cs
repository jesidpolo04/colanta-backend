namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.Domain;
    public class EFOrderStatus
    {
        public int id { get; set; }
        public string status { get; set; }

        public void setEFOrderStatusFromOrderStatus(OrderStatus orderStatus)
        {
            this.id = orderStatus.id;
            this.status = orderStatus.status;
        }

        public OrderStatus getOrderStatusFromEfOrderStatus()
        {
            OrderStatus orderStatus = new OrderStatus();
            orderStatus.id = this.id;
            orderStatus.status = this.status;
            return orderStatus;
        }
    }
}
