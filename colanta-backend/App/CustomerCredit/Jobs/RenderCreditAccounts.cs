namespace colanta_backend.App.CustomerCredit.Jobs
{
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System;
    using Shared.Domain;
    using Shared.Application;
    public class RenderCreditAccounts
    {
        private string processName = "Renderizado de Cupos";
        private CreditAccountsRepository localRepository;
        private CreditAccountsVtexRepository vtexRepository;
        private CreditAccountsSiesaRepository siesaRepository;
        private CustomConsole console;

        public RenderCreditAccounts(
            CreditAccountsRepository localRepository,
            CreditAccountsVtexRepository vtexRepository,
            CreditAccountsSiesaRepository siesaRepository

            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.console = new CustomConsole();
        }

        public async Task Invoke()
        {
            this.console.processStartsAt(this.processName, DateTime.Now);
            try
            {
                CreditAccount[] siesaCreditAccounts = await this.siesaRepository.getAllAccounts();
                foreach (CreditAccount siesaCreditAccount in siesaCreditAccounts)
                {
                    try
                    {
                        CreditAccount localCreditAccount = await localRepository.getCreditAccountByDocumentAndBusiness(siesaCreditAccount.document, siesaCreditAccount.business);
                        if (localCreditAccount == null)
                        {
                            CreditAccount vtexAccount = await vtexRepository.getCreditAccountByVtexId(siesaCreditAccount.document + "_" + siesaCreditAccount.business);
                            if (vtexAccount != null)
                            {
                                localCreditAccount = await localRepository.saveAccount(siesaCreditAccount);
                                localCreditAccount.vtex_credit_limit = vtexAccount.vtex_credit_limit;
                                localCreditAccount.vtex_current_credit = vtexAccount.vtex_current_credit;
                                if (localCreditAccount.vtex_current_credit < localCreditAccount.current_credit)
                                {
                                    decimal totalAport = localCreditAccount.current_credit - localCreditAccount.vtex_credit_limit;
                                    CreditAccount newVtexCreditAccount = await vtexRepository.changeCreditLimit(localCreditAccount.vtex_credit_limit + totalAport, localCreditAccount.vtex_id);
                                    localCreditAccount.vtex_credit_limit = newVtexCreditAccount.vtex_credit_limit;
                                    localCreditAccount.vtex_current_credit = newVtexCreditAccount.vtex_current_credit;
                                    await localRepository.updateCreditAccount(localCreditAccount);
                                }
                                if (localCreditAccount.vtex_current_credit > localCreditAccount.current_credit)
                                {
                                    decimal totalDue = localCreditAccount.vtex_current_credit - localCreditAccount.current_credit;
                                    await vtexRepository.generateInvoice(totalDue, localCreditAccount.vtex_id);
                                    localCreditAccount.vtex_current_credit = localCreditAccount.current_credit;
                                    await localRepository.updateCreditAccount(localCreditAccount);
                                }
                            }
                            else
                            {
                                localCreditAccount = await localRepository.saveAccount(siesaCreditAccount);
                                vtexAccount = await vtexRepository.SaveCreditAccount(localCreditAccount);
                                localCreditAccount.vtex_id = vtexAccount.vtex_id;
                                localCreditAccount.vtex_credit_limit = vtexAccount.vtex_credit_limit;
                                localCreditAccount.vtex_current_credit = vtexAccount.vtex_current_credit;

                                if (localCreditAccount.credit_limit != localCreditAccount.current_credit)
                                {
                                    decimal totalDue = localCreditAccount.credit_limit - localCreditAccount.current_credit;
                                    await vtexRepository.generateInvoice(totalDue, localCreditAccount.vtex_id);
                                }
                                await localRepository.updateCreditAccount(localCreditAccount);
                            }
                        }
                        if (localCreditAccount != null)
                        {
                            if (localCreditAccount.current_credit > siesaCreditAccount.current_credit)
                            {
                                decimal totalDue = localCreditAccount.current_credit - siesaCreditAccount.current_credit;
                                await vtexRepository.generateInvoice(totalDue, localCreditAccount.vtex_id);
                                localCreditAccount.current_credit = siesaCreditAccount.current_credit;
                                localCreditAccount.vtex_current_credit = siesaCreditAccount.current_credit;
                                await localRepository.updateCreditAccount(localCreditAccount);
                            }
                            if (localCreditAccount.current_credit < siesaCreditAccount.current_credit)
                            {
                                decimal totalCanceled = siesaCreditAccount.current_credit - localCreditAccount.current_credit;
                                CreditAccount newVtexCreditAccount = await vtexRepository.changeCreditLimit(localCreditAccount.vtex_credit_limit + totalCanceled, localCreditAccount.vtex_id);
                                localCreditAccount.vtex_credit_limit = newVtexCreditAccount.vtex_credit_limit;
                                localCreditAccount.vtex_current_credit = newVtexCreditAccount.vtex_current_credit;
                                localCreditAccount.current_credit = siesaCreditAccount.current_credit;
                                await localRepository.updateCreditAccount(localCreditAccount);
                            }
                        }
                    }
                    catch (VtexException exception)
                    {

                    }
                }
            }
            catch(SiesaException exception) 
            {

            }
            catch (Exception exception)
            {

            }
            this.console.processEndstAt(this.processName, DateTime.Now);
        }
    }
}
