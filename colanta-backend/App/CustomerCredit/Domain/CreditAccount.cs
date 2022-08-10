namespace colanta_backend.App.CustomerCredit.Domain
{
    using Users.Domain;
    using System;
    public class CreditAccount
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string document { get; set; }
        public string? email { get; set; }
        public decimal credit_limit { get; set; }
        public decimal vtex_credit_limit { get; set; }
        public decimal current_credit { get; set; }
        public decimal vtex_current_credit { get; set; }
        public string business { get; set; }
        public bool is_active { get; set; }

        public decimal currentCreditDiff(decimal newCredit)
        {
            return Math.Abs(this.current_credit - newCredit); 
        }

        public bool currentCreditIsLowThan(decimal newCredit)
        {
            if (this.current_credit < newCredit) return true;
            else return false;
        }

        public bool currentCreditIsMoreThan(decimal newCredit)
        {
            if(this.current_credit > newCredit) return true;
            else return false ;
        }

        public decimal newVtexBalance(decimal extraBalance)
        {
            this.vtex_current_credit += extraBalance;
            this.vtex_credit_limit += extraBalance;
            return this.vtex_current_credit;
        }

        public decimal reduceBalance(decimal valueToReduce)
        {
            this.vtex_current_credit -= valueToReduce;
            this.current_credit -= valueToReduce;
            return this.current_credit;
        }
    }
}
