namespace colanta_backend.App.Orders.SiesaOrders.Domain
{
    public class UtilitiesSiesaOrders
    {
        public static SiesaOrder generateCancelledSiesaOrder()
        {
            var siesaOrder = new SiesaOrder();
            siesaOrder.cancelado = true;
            return siesaOrder;
        }
    }
}
