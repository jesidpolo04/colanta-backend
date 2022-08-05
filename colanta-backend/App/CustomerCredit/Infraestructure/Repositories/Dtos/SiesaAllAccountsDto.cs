namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using CustomerCredit.Domain;
    public class SiesaAllAccountsDto
    {
        public SiesaAccountDto[] cuentas { get; set; }
    }

    public class SiesaAccountDto
    {
        public string negocio { get; set; }
        public string documento { get; set; }
        public decimal cupo_limite { get; set; }
        public decimal cupo_actual { get; set; }
        public string? email { get; set; }

        public CreditAccount getCreditAccountFromDto()
        {
            CreditAccount creditAccount = new CreditAccount();
            creditAccount.business = negocio;
            creditAccount.document = documento;
            creditAccount.credit_limit = cupo_limite;
            creditAccount.current_credit = cupo_actual;
            creditAccount.email = email;
            return creditAccount;
        }
    }
}
