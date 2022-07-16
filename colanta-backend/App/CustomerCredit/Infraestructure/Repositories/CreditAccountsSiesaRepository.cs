using colanta_backend.App.CustomerCredit.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Shared.Domain;
    using Microsoft.Extensions.Configuration;
    public class CreditAccountsSiesaRepository : Domain.CreditAccountsSiesaRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;

        public CreditAccountsSiesaRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }
        public async Task<decimal> getAccountByDocumentAndBusiness(string document, string business)
        {
            string endpoint = "/balance_cuenta";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaAccountCurrentCreditDto currentCreditDto = JsonSerializer.Deserialize<SiesaAccountCurrentCreditDto>(siesaResponseBody);
            return currentCreditDto.cupo_actual;
        }


        public async Task<CreditAccount[]> getAllAccounts()
        {
            string endpoint = "/cuentas";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaAllAccountsDto siesaAllAccounts = JsonSerializer.Deserialize<SiesaAllAccountsDto>(siesaResponseBody);
            List<CreditAccount> creditAccounts = new List<CreditAccount>();
            foreach (SiesaAccountDto siesaAccount in siesaAllAccounts.cuentas)
            {
                creditAccounts.Add(siesaAccount.getCreditAccountFromDto());
            }
            return creditAccounts.ToArray();
        }
    }
}
