namespace colanta_backend.App.CustomerCredit.Domain
{
    using System.Threading.Tasks;
    public interface CreditAccountsVtexRepository
    {
        Task<CreditAccount> getCreditAccountByVtexId(string vtexId);
        Task<CreditAccount> SaveCreditAccount(CreditAccount creditAccount);
        Task<CreditAccount> closeCreditAccount(CreditAccount creditAccount);
        Task<CreditAccount> changeCreditLimit(decimal newCreditLimit, string vtexCreditAccountId);
        Task generateInvoice(decimal value, string vtexCreditAccountId);
        Task paidInvoice(Invoice invoice);
        Task<Invoice[]> getAllAccountInvoices(string accountVtexId, string status);
    }
}
