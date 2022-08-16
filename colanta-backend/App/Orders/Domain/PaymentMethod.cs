namespace colanta_backend.App.Orders.Domain
{
    public class PaymentMethod
    {
        public readonly string name;
        public readonly string id;

        public PaymentMethod(string id, string name)
        {
            this.name = name;
            this.id = id;
        }

        public bool isEqual(PaymentMethod comparePaymentMehod)
        {
            bool isEqual = true;
            isEqual = this.name.Equals(comparePaymentMehod.name) ? isEqual : false;
            isEqual = this.id.Equals(comparePaymentMehod.id) ? isEqual : false;

            return isEqual;
        }

        public bool isPromissory()
        {
            if (this.id == PaymentMethods.EFECTIVO.id) return true;
            if (this.id == PaymentMethods.CONTRAENTREGA.id) return true;
            if (this.id == PaymentMethods.CARD_PROMISSORY.id) return true;
            else return false;
        }
    }
}
