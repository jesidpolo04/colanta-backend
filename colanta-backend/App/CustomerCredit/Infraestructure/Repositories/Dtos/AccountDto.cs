namespace colanta_backend.App.CustomerCredit.Infraestructure { 
    using CustomerCredit.Domain;
    public class AccountDto
    {
        public string id { get; set; }
        public decimal balance { get; set; }
        public string document { get; set; }
        public string status { get; set; }
        public string documentType{ get; set; }
        public decimal creditLimit { get; set; }
        public string updatedAt { get; set; }
        public string createdAt { get; set; }
        public string description { get; set; }
        public decimal availableCredit { get; set; }
        public decimal preAuthorizedCredit { get; set; }
        public string email { get; set; }
        public decimal tolerance { get; set; }
        public decimal availableBalance { get; set; }

        public CreditAccount getCreditAccountFromDto()
        {
            CreditAccount creditAccount = new CreditAccount();
            creditAccount.vtex_id = this.id;
            creditAccount.vtex_credit_limit = this.creditLimit;
            creditAccount.vtex_current_credit = this.availableBalance;
            creditAccount.document = this.document;
            creditAccount.email = this.email;
            return creditAccount;
        }
    }
}
