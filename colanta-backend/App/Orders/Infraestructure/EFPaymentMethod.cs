namespace colanta_backend.App.Orders.Infraestructure
{
    using Orders.Domain;
    public class EFPaymentMethod
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_promissory { get; set; }
        public string vtex_id { get; set; }
        public void setEfPaymentFromPayment(PaymentMethod paymentMethod)
        {
            this.id = paymentMethod.id;
            this.name = paymentMethod.name;
            this.is_promissory = paymentMethod.is_promissory;
            this.vtex_id = paymentMethod.vtex_id;
        }

        public PaymentMethod getPaymentFromEfPayment()
        {
            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.id = this.id;
            paymentMethod.name  = this.name;
            paymentMethod.is_promissory = this.is_promissory;
            paymentMethod.vtex_id  = this.vtex_id;
            return paymentMethod;
        }
    }
}
