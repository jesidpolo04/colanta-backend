namespace colanta_backend.App.CustomerCredit.Domain
{
    using Users.Domain;
    public class CreditAccount
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string document { get; set; }
        public string email { get; set; }
        public decimal credit_limit { get; set; }
        public decimal vtex_credit_limit { get; set; }
        public decimal current_credit { get; set; }
        public decimal vtex_current_credit { get; set; }
        public string business { get; set; }
        public bool is_active { get; set; }

        public CreditAccount transaction(decimal value)
        {
            if(current_credit + value < 0)
            {
                throw new InvalidOperationException(value, this.credit_limit, this.current_credit, "La operación resultante da como resultado un saldo negativo: " + (current_credit + value));
            }
            if (current_credit + value > credit_limit)
            {
                throw new InvalidOperationException(value, this.credit_limit, this.current_credit, "La operación resultante da como resultado un saldo mayor al limite: " + (current_credit + value));
            }
            this.current_credit = current_credit + value;
            return this;
        }
    }
}
