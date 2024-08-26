namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.Domain;
    public class EFCanceledOrder
    {
        public int Id { get; set; }
        public string VtexOrderId { get; set; }

        public void setEfCanceledOrder(CanceledOrder order)
        {
            Id = order.Id;
            VtexOrderId = order.VtexOrderId;
        }

        public CanceledOrder getCanceledOrder()
        {
            return new CanceledOrder(){
                Id = this.Id,
                VtexOrderId = this.VtexOrderId
            };
        }
    }
}
