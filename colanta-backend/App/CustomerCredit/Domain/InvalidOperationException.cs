namespace colanta_backend.App.CustomerCredit.Domain
{
    using System;
    public class InvalidOperationException : Exception
    {
        public decimal value;
        public decimal credit_limit;
        public decimal current_credit;
        public InvalidOperationException(decimal value, decimal credit_limit, decimal current_credit, string message) : base(message)
        {
            this.value = value;
            this.credit_limit = credit_limit;
            this.current_credit = current_credit;
        }
    }
}
