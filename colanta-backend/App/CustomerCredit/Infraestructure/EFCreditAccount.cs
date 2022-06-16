namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using Users.Infraestructure;
    using Users.Domain;
    using CustomerCredit.Domain;
    public class EFCreditAccount
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public string document { get; set; }
        public string email { get; set; }
        public int user_id { get; set; }
        public decimal credit_limit { get; set; }
        public decimal vtex_credit_limit { get; set; }
        public decimal vtex_current_credit { get; set; }
        public decimal current_credit { get; set; }
        public string business { get; set; }
        public bool is_active { get; set; }

        public CreditAccount getCreditAccountFromEfCreditAccount()
        {
            CreditAccount creditAccount = new CreditAccount();
            creditAccount.credit_limit = this.credit_limit;
            creditAccount.vtex_credit_limit = this.vtex_credit_limit;   
            creditAccount.business = this.business;
            creditAccount.vtex_id = this.vtex_id;
            creditAccount.document = this.document;
            creditAccount.email = this.email;
            creditAccount.current_credit = this.current_credit;
            creditAccount.vtex_current_credit = this.vtex_current_credit;
            creditAccount.id = this.id;
            creditAccount.is_active = this.is_active;

            return creditAccount;
        }
        public void setEfCreditAccountFromCreditAccount(CreditAccount creditAccount)
        {
            this.credit_limit = creditAccount.credit_limit;
            this.vtex_credit_limit = creditAccount.vtex_credit_limit;
            this.business = creditAccount.business;
            this.vtex_id = creditAccount.vtex_id;
            this.document = creditAccount.document;
            this.email = creditAccount.email;
            this.current_credit = creditAccount.current_credit;
            this.vtex_current_credit = creditAccount.vtex_current_credit;
            this.id = creditAccount.id;
            this.is_active = creditAccount.is_active;
        }
    }
}
