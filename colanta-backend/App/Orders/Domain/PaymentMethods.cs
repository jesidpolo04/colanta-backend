namespace colanta_backend.App.Orders.Domain
{
    public class PaymentMethods
    {
        public readonly static PaymentMethod CUSTOMER_CREDIT = new PaymentMethod("64", "Customer Credit");
        public readonly static PaymentMethod GIFTCARD = new PaymentMethod("64", "Customer Credit");
        public readonly static PaymentMethod EFECTIVO = new PaymentMethod("201", "Efectivo");
        public  static PaymentMethod CONTRAENTREGA = new PaymentMethod("201", "Contra entrega");
        public readonly static PaymentMethod WOMPI = new PaymentMethod("111", "WompiCo");
        public readonly static PaymentMethod CARD_PROMISSORY = new PaymentMethod("601", "CardPromissory");

    }
}
