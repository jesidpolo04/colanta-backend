namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using Users.Infraestructure;
    using Users.Domain;
    using CustomerCredit.Domain;
    public class EFCreditAccount
    {
        public int id { get; set; }
        public string vtex_id { get; set; }
        public EFUser user { get; set; }
        public int user_id { get; set; }
        public decimal credit_limit { get; set; }
        public string business { get; set; }
        public bool is_active { get; set; }

        public CreditAccount getCreditAccountFromEfCreditAccount()
        {
            CreditAccount creditAccount = new CreditAccount();
            creditAccount.credit_limit = this.credit_limit;
            creditAccount.business = this.business;
            creditAccount.vtex_id = this.vtex_id;
            creditAccount.user_id = this.user_id;
            creditAccount.id = this.id;
            creditAccount.is_active = this.is_active;

            User user = new User();
            user.id = this.user.id;
            user.document = this.user.document;
            user.document_type = this.user.document_type;
            user.email = this.user.email;
            user.name = this.user.name;
            user.telephone = this.user.telephone;
            user.client_type = this.user.client_type;

            creditAccount.user = user;
            return creditAccount;
        }
        public void setEfCreditAccountFromCreditAccount(CreditAccount creditAccount)
        {
            this.credit_limit = creditAccount.credit_limit;
            this.business = creditAccount.business;
            this.vtex_id = creditAccount.vtex_id;
            this.user_id = creditAccount.user_id;
            this.id = creditAccount.id;
            this.is_active = creditAccount.is_active;
        }
    }
}
