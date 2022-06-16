using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace colanta_backend.App.CustomerCredit
{
    using CustomerCredit.Domain;
    using System.Threading.Tasks;

    [Route("api")]
    [ApiController]
    public class CustomerCreditController : ControllerBase
    {
        private CreditAccountsRepository localRepository;
        private CreditAccountsVtexRepository vtexRepository;
        private CreditAccountsSiesaRepository siesaRepository;
        public CustomerCreditController(
            CreditAccountsRepository localRepository,
            CreditAccountsVtexRepository vtexRepository,
            CreditAccountsSiesaRepository siesaRepository

            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        [Route("creditAccount/{creditAccountVtexId}")]
        [HttpGet]
        public async Task<object> updateCurrentCreditOfAccount(string creditAccountVtexId)
        {
            System.Console.WriteLine(creditAccountVtexId.ToString());
            CreditAccount localAccount = await localRepository.getCreditAccountByVtexId(creditAccountVtexId);
            decimal accountCurrentCredit = await siesaRepository.getAccountByDocumentAndBusiness(localAccount.document, localAccount.business);
            System.Console.WriteLine(accountCurrentCredit.ToString());
            if (localAccount.current_credit > accountCurrentCredit)
            {
                decimal totalDue = localAccount.current_credit - accountCurrentCredit;
                await vtexRepository.generateInvoice(totalDue, localAccount.vtex_id);
                localAccount.current_credit = accountCurrentCredit;
                localAccount.vtex_current_credit = accountCurrentCredit;
                await localRepository.updateCreditAccount(localAccount);
            }
            if (localAccount.current_credit < accountCurrentCredit)
            {
                decimal totalAport = accountCurrentCredit - localAccount.current_credit;
                CreditAccount newVtexAccount = await vtexRepository.changeCreditLimit(localAccount.vtex_credit_limit + totalAport, localAccount.vtex_id);
                localAccount.current_credit = accountCurrentCredit;
                localAccount.vtex_current_credit = newVtexAccount.vtex_current_credit;
                localAccount.vtex_credit_limit = newVtexAccount.vtex_credit_limit;
                await localRepository.updateCreditAccount(localAccount);
            }
            object response = new
            {
                accountCurrentCredit = accountCurrentCredit
            };
            return response;
        }
    }
}
