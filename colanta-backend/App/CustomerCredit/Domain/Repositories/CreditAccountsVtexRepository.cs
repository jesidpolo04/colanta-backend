namespace colanta_backend.App.CustomerCredit.Domain
{
    using System.Threading.Tasks;
    public interface CreditAccountsVtexRepository
    {
        Task<CreditAccount> getCreditAccountByDocumentAndBusiness(string document, string business);
        Task<CreditAccount> updateCreditAccountByDocumentAndBusiness(string document, string business);
        Task<CreditAccount> SaveCreditAccount(CreditAccount creditAccount);
        Task<CreditAccount> closeCreditAccount(CreditAccount creditAccount);
    }
}
