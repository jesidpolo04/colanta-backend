namespace colanta_backend.App.CustomerCredit.Domain
{
    using Users.Domain;
    public class CreditAccount
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public User user { get; set; }
        public int user_id { get; set; }
        public decimal credit_limit { get; set; }
        public string business { get; set; }
        public bool is_active { get; set; }
    }
}
