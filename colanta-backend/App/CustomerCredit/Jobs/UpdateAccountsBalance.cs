namespace colanta_backend.App.CustomerCredit.Jobs
{
    using CustomerCredit.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public class UpdateAccountsBalance
    {
        private string processName = "Actualizado de saldos (Cupo Lacteo)"; 
        private CreditAccountsRepository localRepository;
        private CreditAccountsSiesaRepository siesaRepository;
        private CreditAccountsVtexRepository vtexRepository;
        private CustomConsole console = new CustomConsole();

        public UpdateAccountsBalance(CreditAccountsRepository localRepository, CreditAccountsSiesaRepository siesaRepository, CreditAccountsVtexRepository vtexRepository)
        {
            this.localRepository = localRepository;
            this.siesaRepository = siesaRepository;
            this.vtexRepository = vtexRepository;
        }

        public async Task Invoke()
        {
            console.processStartsAt(this.processName, DateTime.Now);
            CreditAccount[] localAccounts = await this.localRepository.getAllAccounts();
            foreach(CreditAccount localAccount in localAccounts)
            {
                try
                {
                    decimal newBalance = await this.siesaRepository.getAccountByDocumentAndBusiness(localAccount.document, localAccount.business);
                    decimal creditDiff = localAccount.currentCreditDiff(newBalance);
                    if (localAccount.currentCreditIsLowThan(newBalance))
                    {
                        vtexRepository.changeCreditLimit(localAccount.newVtexBalance(creditDiff), localAccount.vtex_id).Wait();
                        localRepository.updateCreditAccount(localAccount).Wait();
                    }
                    if (localAccount.currentCreditIsMoreThan(newBalance))
                    {
                        vtexRepository.generateInvoice(creditDiff, localAccount.vtex_id).Wait();
                        localAccount.reduceBalance(creditDiff);
                        localRepository.updateCreditAccount(localAccount).Wait();
                    }
                }
                catch (Exception exception)
                {
                    console.throwException(exception.Message);
                }
            }
            console.processStartsAt(this.processName, DateTime.Now);
        }
    }
}
