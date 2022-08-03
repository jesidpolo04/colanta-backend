namespace colanta_backend.App.CustomerCredit.Jobs
{
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System;
    using Shared.Domain;
    using System.Text.Json;
    using Shared.Application;
    using System.Collections.Generic;
    public class RenderCreditAccounts : IDisposable
    {
        private string processName = "Renderizado de Cupos";
        private CreditAccountsRepository localRepository;
        private CreditAccountsVtexRepository vtexRepository;
        private CreditAccountsSiesaRepository siesaRepository;
        private ILogger logger;
        private IProcess process;
        private IRenderAccountsMail mail;
        private CustomConsole console;
        private JsonSerializerOptions jsonOptions;

        private int totalObtained = 0;
        private List<CreditAccount> loadedAccounts = new List<CreditAccount>();
        private List<CreditAccount> updatedAccounts = new List<CreditAccount>();
        private List<CreditAccount> failedAccounts = new List<CreditAccount>();
        private List<CreditAccount> notProccecedCreditAccounts = new List<CreditAccount>();
        private List<Detail> details = new List<Detail>();
        public RenderCreditAccounts(
            CreditAccountsRepository localRepository,
            CreditAccountsVtexRepository vtexRepository,
            CreditAccountsSiesaRepository siesaRepository,
            ILogger logger,
            IProcess process,
            IRenderAccountsMail mail
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.logger = logger;
            this.process = process;
            this.mail = mail;
            this.console = new CustomConsole();
            this.jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            this.console.processStartsAt(this.processName, DateTime.Now);
            try
            {
                CreditAccount[] siesaCreditAccounts = await this.siesaRepository.getAllAccounts();
                this.totalObtained = siesaCreditAccounts.Length;
                foreach (CreditAccount siesaCreditAccount in siesaCreditAccounts)
                {
                    try
                    {
                        CreditAccount localCreditAccount = await localRepository.getCreditAccountByDocumentAndBusiness(siesaCreditAccount.document, siesaCreditAccount.business);
                        
                        if (localCreditAccount != null)
                        {
                            if (localCreditAccount.current_credit > siesaCreditAccount.current_credit)
                            {
                                decimal totalDue = localCreditAccount.current_credit - siesaCreditAccount.current_credit;
                                await vtexRepository.generateInvoice(totalDue, localCreditAccount.vtex_id);
                                localCreditAccount.current_credit = siesaCreditAccount.current_credit;
                                localCreditAccount.vtex_current_credit = siesaCreditAccount.current_credit;
                                await localRepository.updateCreditAccount(localCreditAccount);

                                this.updatedAccounts.Add(localCreditAccount);
                                this.details.Add(new Detail("vtex", "actualizar cuenta", null, null, true));
                            }
                            if (localCreditAccount.current_credit < siesaCreditAccount.current_credit)
                            {
                                decimal totalCanceled = siesaCreditAccount.current_credit - localCreditAccount.current_credit;
                                CreditAccount newVtexCreditAccount = await vtexRepository.changeCreditLimit(localCreditAccount.vtex_credit_limit + totalCanceled, localCreditAccount.vtex_id);
                                localCreditAccount.vtex_credit_limit = newVtexCreditAccount.vtex_credit_limit;
                                localCreditAccount.vtex_current_credit = newVtexCreditAccount.vtex_current_credit;
                                localCreditAccount.current_credit = siesaCreditAccount.current_credit;
                                await localRepository.updateCreditAccount(localCreditAccount);

                                this.updatedAccounts.Add(localCreditAccount);
                                this.details.Add(new Detail("vtex", "actualizar cuenta", null, null, true));

                            }
                            if (localCreditAccount.current_credit == siesaCreditAccount.current_credit)
                            {
                                this.notProccecedCreditAccounts.Add(localCreditAccount);
                            }
                        }

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

                                    this.updatedAccounts.Add(localCreditAccount);
                                    this.details.Add(new Detail("vtex", "actualizar cuenta", null, null, true));
                                }
                                if (localCreditAccount.vtex_current_credit > localCreditAccount.current_credit)
                                {
                                    decimal totalDue = localCreditAccount.vtex_current_credit - localCreditAccount.current_credit;
                                    await vtexRepository.generateInvoice(totalDue, localCreditAccount.vtex_id);
                                    localCreditAccount.vtex_current_credit = localCreditAccount.current_credit;
                                    await localRepository.updateCreditAccount(localCreditAccount);

                                    this.updatedAccounts.Add(localCreditAccount);
                                    this.details.Add(new Detail("vtex", "actualizar cuenta", null, null, true));
                                }
                                if (localCreditAccount.vtex_current_credit == localCreditAccount.credit_limit)
                                {
                                    this.notProccecedCreditAccounts.Add(localCreditAccount);
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

                                this.loadedAccounts.Add(localCreditAccount);
                                this.details.Add(new Detail("vtex", "crear cuenta", null, null, true));
                            }
                        }
                    }
                    catch (VtexException exception)
                    {
                        this.failedAccounts.Add(siesaCreditAccount);
                        this.console.throwException(exception.Message);
                        await this.logger.writelog(exception);
                    }
                }
            }
            catch(SiesaException exception) 
            {
                this.console.throwException(exception.Message);
                await this.logger.writelog(exception);
                this.details.Add(new Detail("siesa", exception.requestUrl, exception.responseBody, exception.Message, false));
            }
            catch (Exception exception)
            {
                this.console.throwException(exception.Message);
                await this.logger.writelog(exception);
            }
            this.process.Log(
                this.processName,
                this.loadedAccounts.Count + this.updatedAccounts.Count,
                this.failedAccounts.Count,
                this.notProccecedCreditAccounts.Count,
                this.totalObtained,
                JsonSerializer.Serialize(this.details, jsonOptions));
            this.console.processEndstAt(processName, DateTime.Now);
            this.mail.sendMail(this.loadedAccounts, this.updatedAccounts, this.failedAccounts);
        }

        public void Dispose()
        {
            this.loadedAccounts.Clear();
            this.updatedAccounts.Clear();
            this.failedAccounts.Clear();
            this.notProccecedCreditAccounts.Clear();
            this.totalObtained = 0;
            this.details.Clear();
        }

    }
}
