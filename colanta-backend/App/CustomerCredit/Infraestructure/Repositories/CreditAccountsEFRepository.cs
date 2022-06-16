namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using Microsoft.Extensions.Configuration;
    using Shared.Infraestructure;
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System.Text.Json;
    using System.Linq;
    using System.Collections.Generic;

    public class CreditAccountsEFRepository : CreditAccountsRepository
    {
        private ColantaContext dbContext;

        public CreditAccountsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public async Task<CreditAccount[]> getAllAccounts()
        {
            EFCreditAccount[] efCreditAccounts = this.dbContext.CreditAccounts.ToArray();
            List<CreditAccount> creditAccounts = new List<CreditAccount>();
            foreach(EFCreditAccount efCreditAccount in efCreditAccounts)
            {
                creditAccounts.Add(efCreditAccount.getCreditAccountFromEfCreditAccount());
            }
            return creditAccounts.ToArray();
        }

        public async Task<CreditAccount> getCreditAccountByDocumentAndBusiness(string document, string business)
        {
            var efCreditAccounts = this.dbContext.CreditAccounts.Where(creditAccount => creditAccount.business == business && creditAccount.document == document);
            if(efCreditAccounts.ToArray().Length > 0)
            {
                return efCreditAccounts.First().getCreditAccountFromEfCreditAccount();
            }
            return null;
        }

        public async Task<CreditAccount> getCreditAccountByVtexId(string vtexId)
        {
            var efCreditAccounts = this.dbContext.CreditAccounts.Where(creditAccount => creditAccount.vtex_id == vtexId);
            if (efCreditAccounts.ToArray().Length > 0)
            {
                return efCreditAccounts.First().getCreditAccountFromEfCreditAccount();
            }
            return null;
        }

        public Task<CreditAccount[]> getDeltaAccounts(CreditAccount[] currentAccounts)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CreditAccount> saveAccount(CreditAccount creditAccount)
        {
            EFCreditAccount efCreditAccount = new EFCreditAccount();
            efCreditAccount.setEfCreditAccountFromCreditAccount(creditAccount);
            this.dbContext.Add(efCreditAccount);
            this.dbContext.SaveChanges();
            return await this.getCreditAccountByDocumentAndBusiness(creditAccount.document, creditAccount.business);
        }

        public async Task<CreditAccount> updateCreditAccount(CreditAccount creditAccount)
        {
            EFCreditAccount efCreditAccount = this.dbContext.CreditAccounts.Find(creditAccount.id);
            efCreditAccount.setEfCreditAccountFromCreditAccount(creditAccount);
            this.dbContext.SaveChanges();
            return creditAccount;
        }

        public async Task<CreditAccount[]> updateCreditAccounts(CreditAccount[] creditAccounts)
        {
            foreach(CreditAccount creditAccount in creditAccounts)
            {
                EFCreditAccount efCreditAccount = this.dbContext.CreditAccounts.Find(creditAccount.id);
                efCreditAccount.setEfCreditAccountFromCreditAccount(creditAccount);
            }
            this.dbContext.SaveChanges();
            return creditAccounts;
        }
    }
}
