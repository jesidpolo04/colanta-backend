namespace colanta_backend.App.CustomerCredit.Jobs
{
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System;
    public class RenderCreditAccounts
    {
        private CreditAccountsRepository localRepository;
        private CreditAccountsVtexRepository vtexRepository;
        private CreditAccountsSiesaRepository siesaRepository;

        public RenderCreditAccounts(
            CreditAccountsRepository localRepository,
            CreditAccountsVtexRepository vtexRepository,
            CreditAccountsSiesaRepository siesaRepository

            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task Invoke()
        {
            try
            {
                CreditAccount[] allSiesaAccounts = await this.siesaRepository.getAllAccounts();
                CreditAccount[] deltaAccounts = await this.localRepository.getDeltaAccounts(allSiesaAccounts);
                
                if(deltaAccounts.Length > 0)
                {
                    foreach (CreditAccount deltaAccount in deltaAccounts)
                    {
                        try
                        {
                            deltaAccount.is_active = false;
                            await this.vtexRepository.closeCreditAccount(deltaAccount);
                        }
                        catch (Exception exception)
                        {
                        }
                    }
                    await this.localRepository.updateCreditAccounts(deltaAccounts);
                }
                

                foreach (CreditAccount siesaAccount in allSiesaAccounts)
                {
                    try
                    {
                        CreditAccount? localAccount = await this.localRepository.getCreditAccountByDocumentAndBusiness(siesaAccount.user.document, siesaAccount.business);
                        if(localAccount == null)
                        {
                            localAccount = await this.localRepository.saveAccount(siesaAccount);
                            CreditAccount vtexAccount = await this.vtexRepository.SaveCreditAccount(localAccount);
                            localAccount.vtex_id = vtexAccount.vtex_id;
                            await this.localRepository.updateCreditAccount(localAccount);
                        }
                        if(localAccount != null)
                        {
                        }
                    }
                    catch(Exception exception)
                    {
                    }
                }
            }
            catch
            {

            }
        }
    }
}
