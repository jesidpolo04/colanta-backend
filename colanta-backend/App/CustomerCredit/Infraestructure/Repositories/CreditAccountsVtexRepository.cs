namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using Microsoft.Extensions.Configuration;
    using CustomerCredit.Domain;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Shared.Domain;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    public class CreditAccountsVtexRepository : Domain.CreditAccountsVtexRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;
        public CreditAccountsVtexRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnvironment = configuration["MercolantaEnvironment"];

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.setCredentialHeaders();
        }

        private void setCredentialHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppToken");
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppKey");

            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);

        }

        public async Task<CreditAccount> changeCreditLimit(decimal newCreditLimit, string vtexCreditAccountId)
        {
            string endpoint = "/api/creditcontrol/accounts/" + vtexCreditAccountId + "/creditlimit";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            object requestBody = new { value = newCreditLimit };
            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PutAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("No fue posible actualizar el limite de crédito de la cuenta: " + vtexCreditAccountId + " Vtex Respondio con status: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            JObject vtexCreditAccount = JObject.Parse(vtexResponseBody);
            CreditAccount creditAccount = new CreditAccount();

            creditAccount.vtex_id = (string) vtexCreditAccount["id"];
            creditAccount.vtex_credit_limit = (decimal)vtexCreditAccount["creditLimit"];
            creditAccount.vtex_current_credit = (decimal)vtexCreditAccount["availableBalance"];
            creditAccount.document = (string)vtexCreditAccount["document"];

            return creditAccount;
        }

        public Task<CreditAccount> closeCreditAccount(CreditAccount creditAccount)
        {
            throw new System.NotImplementedException();
        }

        public async Task generateInvoice(decimal value, string vtexCreditAccountId)
        {
            string transactionId = await this.generateTransaction(value, vtexCreditAccountId);
            string endpoint = "/api/creditcontrol/accounts/"+vtexCreditAccountId+"/transactions/"+transactionId+"/settlement";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            object requestBody = new
            {
                value = value
            };
            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage invoiceVtexResponse = await this.httpClient.PutAsync(url, httpContent);
            if (!invoiceVtexResponse.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Excepcion - Vtex Response");
                System.Console.WriteLine(await invoiceVtexResponse.Content.ReadAsStringAsync());
                throw new VtexException("No fue posible crear la factura, Vtex respondió con status" + invoiceVtexResponse.StatusCode);
            }
        }

        public async Task<string> generateTransaction(decimal value, string vtexCreditAccountId)
        {
            string endpoint = "/api/creditcontrol/accounts/" + vtexCreditAccountId + "/transactions";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            object requestBody = new
            {
                value = value
            };
            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage preAuthorizationVtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!preAuthorizationVtexResponse.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Excepcion - Vtex Response");
                System.Console.WriteLine(await preAuthorizationVtexResponse.Content.ReadAsStringAsync());
                throw new VtexException("No fue posible crear la transacción (pre-autorización) en Vtex, respondió con status" + preAuthorizationVtexResponse.StatusCode);
            }
            string preAuthorizationVtexResponseBody = await preAuthorizationVtexResponse.Content.ReadAsStringAsync();
            JObject preAuthorization = JObject.Parse(preAuthorizationVtexResponseBody);
            string preAuthorizationId = (string)preAuthorization["id"];
            return preAuthorizationId;
        }

        public async Task<Invoice[]> getAllAccountInvoices(string accountVtexId, string status)
        {
            string endpoint = "/api/creditcontrol/accounts/"+accountVtexId+"/invoices";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Excepcion - Vtex Response");
                System.Console.WriteLine(await vtexResponse.Content.ReadAsStringAsync());
                throw new VtexException("No fue posible consultar las facturas por cuenta, (cuenta: "+ accountVtexId +") Vtex respondió con status: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            InvoicesByAccountDto invoicesByAccountDto = JsonSerializer.Deserialize<InvoicesByAccountDto>(vtexResponseBody);
            List<Invoice> invoices = new List<Invoice>();
            foreach(InvoiceDto invoiceDto in invoicesByAccountDto.data)
            {
                if(status == "all")
                {
                    invoices.Add(invoiceDto.getInvoiceFromDto());
                }
                if(invoiceDto.status == status)
                {
                    invoices.Add(invoiceDto.getInvoiceFromDto());
                }
            }
            return invoices.ToArray();
        }

        public async Task<CreditAccount> getCreditAccountByVtexId(string vtexId)
        {
            string endpoint = "/api/creditcontrol/accounts/" + vtexId;
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if(vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("Hubo un problema al intentar consultar la cuenta: " + vtexId + "Vtex respondió con status: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            AccountDto accountDto = JsonSerializer.Deserialize<AccountDto>(vtexResponseBody);
            return accountDto.getCreditAccountFromDto();
        }

        public async Task<CreditAccount> SaveCreditAccount(CreditAccount creditAccount)
        {
            string endpoint = "/api/creditcontrol/accounts";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            object requestBody = new
            {
                document = creditAccount.document,
                documentType = creditAccount.business,
                email = creditAccount.email,
                creditLimit = creditAccount.credit_limit,
                description = "",
                tolerance = 0.0
            };
            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("No fue posible crear la cuenta de crédito con documento: " + creditAccount.document + " y email: " + creditAccount.email + ", Vtex respondio con status: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            AccountDto accountDto = JsonSerializer.Deserialize<AccountDto>(vtexResponseBody);
            return accountDto.getCreditAccountFromDto();
        }

        public async Task paidInvoice(string invoiceId, string vtexCreditAccountId)
        {
            string endpoint = "/api/creditcontrol/accounts/"+ vtexCreditAccountId +"/invoices/" + invoiceId;
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            object requestBody = new {
                status = "Paid",
                observation = "",
                paymentLink = ""
            };
            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PutAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("No fue posible pagar la factura: " + invoiceId + "Vtex respondió con status:" + vtexResponse.StatusCode);
            }
        }
    }
}
