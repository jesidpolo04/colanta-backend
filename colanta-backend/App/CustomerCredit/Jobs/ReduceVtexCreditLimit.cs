namespace colanta_backend.App.CustomerCredit.Jobs
{
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System;
    using Shared.Domain;
    public class ReduceVtexCreditLimit
    {
        private CreditAccountsRepository localRepository;
        private CreditAccountsVtexRepository vtexRepository;

        public ReduceVtexCreditLimit(CreditAccountsRepository localRepository, CreditAccountsVtexRepository vtexRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
        }

        public async Task Invoke()
        {
            try
            {
                CreditAccount[] localCreditAccounts = await this.localRepository.getAllAccounts();
                foreach(CreditAccount localCreditAccount in localCreditAccounts)
                {
                    Invoice[] accountInvoices = await vtexRepository.getAllAccountInvoices(localCreditAccount.vtex_id, "Open");
                    foreach(Invoice invoice in accountInvoices)
                    {
                        decimal availableAmountToReduce = localCreditAccount.vtex_credit_limit - localCreditAccount.credit_limit;
                        if (invoice.value <= availableAmountToReduce)
                        {
                            await vtexRepository.changeCreditLimit(invoice.value, invoice.creditAccountId);
                            await vtexRepository.paidInvoice(invoice);
                            localCreditAccount.vtex_credit_limit = localCreditAccount.vtex_credit_limit - invoice.value;
                            await localRepository.updateCreditAccount(localCreditAccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
