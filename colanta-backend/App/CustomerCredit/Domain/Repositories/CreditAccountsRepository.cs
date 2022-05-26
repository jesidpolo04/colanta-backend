namespace colanta_backend.App.CustomerCredit.Domain
{
    using System.Threading.Tasks;
    public interface CreditAccountsRepository
    {
        Task<CreditAccount> getAllAccounts();
        Task<CreditAccount[]> getDeltaAccounts(CreditAccount[] currentAccounts);
        Task<CreditAccount> getCreditAccountByDocumentAndBusiness(string document, string business);
        Task<CreditAccount> saveAccount(CreditAccount creditAccount);
        Task<CreditAccount> updateCreditAccount(CreditAccount creditAccount);
        Task<CreditAccount> updateCreditAccounts(CreditAccount[] creditAccounts);
    }
}
