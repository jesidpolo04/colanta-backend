namespace colanta_backend.App.Orders.Domain
{
    public class OrderVtexStates
    {
        public static string READY_FOR_HANDLING = "ready-for-handling";
        public static string ORDER_COMPLETED = "order-completed";
        public static string HANDLING = "handling";
        public static string WINDOW_TO_CANCEL = "window-to-cancel";
        public static string WAITING_FFMT_AUTHORIZATION = "waiting-ffmt-authorization";
        public static string PAYMENT_PENDING = "payment-pending";
    }
}
