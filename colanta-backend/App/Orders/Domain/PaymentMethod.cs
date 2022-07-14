namespace colanta_backend.App.Orders.Domain
{
    public class PaymentMethod
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool is_promissory { get; set; }
        public string vtex_id { get; set; }
    }
}
