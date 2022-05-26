namespace colanta_backend.App.CustomerCredit.Domain
{
    using System.Threading.Tasks;
    public interface CreditAccountsSiesaRepository
    {
        Task<CreditAccount[]> getAllAccounts();

        Task<CreditAccount> getAccountByDocumentAndBusiness(string document, string business);
    }
}
