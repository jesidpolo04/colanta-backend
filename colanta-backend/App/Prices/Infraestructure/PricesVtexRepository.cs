using colanta_backend.App.Prices.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Prices.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using App.Shared.Domain;
    public class PricesVtexRepository : Domain.PricesVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;

        public PricesVtexRepository(IConfiguration configuration)
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

        public void changeEnvironment(string environment)
        {
            environment = environment.Trim();
            string[] possibleValues = { "mercolanta", "agrocolanta" };

            foreach (string possibleValue in possibleValues)
            {
                if (environment == possibleValue)
                {
                    if (possibleValue == "mercolanta")
                    {
                        this.apiKey = configuration["MercolantaVtexApiKey"];
                        this.apiToken = configuration["MercolantaVtexToken"];
                        this.accountName = configuration["MercolantaAccountName"];
                        this.vtexEnvironment = configuration["MercolantaEnvironment"];
                    }
                    if (possibleValue == "agrocolanta")
                    {
                        this.apiKey = configuration["AgrocolantaVtexApiKey"];
                        this.apiToken = configuration["AgrocolantaVtexToken"];
                        this.accountName = configuration["AgrocolantaAccountName"];
                        this.vtexEnvironment = configuration["AgrocolantaEnvironment"];
                    }
                    this.setCredentialHeaders();
                    return;
                }
            }
            throw new ArgumentOutOfRangeException(paramName: "enviroment", message: "Invalid Enviroment, Only can be: 'mercolanta' or 'agrocolanta'");
        }

        public async Task<Price?> getPriceByVtexId(int? vtexId)
        {
            string accountName = this.accountName;
            string endpoint = "/pricing/prices/" + vtexId;
            string url = "https://api.vtex.com/" + accountName;

            HttpResponseMessage vtexReponse = await this.httpClient.GetAsync(url + endpoint);
            if(vtexReponse.StatusCode != System.Net.HttpStatusCode.OK && vtexReponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("Hubo un problema con Vtex, respondió con status" + vtexReponse.StatusCode);
            }
            if(vtexReponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string vtexResponseBody = await vtexReponse.Content.ReadAsStringAsync();
            GetPriceDto getPriceDto = JsonSerializer.Deserialize<GetPriceDto>(vtexResponseBody);
            return getPriceDto.getPriceFromDto();
        }

        public async Task<Price> savePrice(Price price)
        {
            string accountName = this.accountName;
            string endpoint = "/pricing/prices/" + price.sku.vtex_id;
            string url = "https://api.vtex.com/" + accountName;

            SaveVtexPriceDto requestBody = new SaveVtexPriceDto();
            requestBody.markup = 0;
            requestBody.costPrice = null;
            requestBody.basePrice = price.price;

            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage vtexResponse = await this.httpClient.PutAsync(url + endpoint, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("Hubo un problema con Vtex, respondió con status" + vtexResponse.StatusCode);
            }
            return price;
        }

        public async Task<Price> updatePrice(Price price)
        {
            string accountName = this.accountName;
            string endpoint = "/pricing/prices/" + price.sku.vtex_id;
            string url = "https://api.vtex.com/" + accountName;

            SaveVtexPriceDto requestBody = new SaveVtexPriceDto();
            requestBody.markup = 0;
            requestBody.costPrice = null;
            requestBody.basePrice = price.price;

            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage vtexResponse = await this.httpClient.PutAsync(url + endpoint, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("Hubo un problema con Vtex, respondió con status" + vtexResponse.StatusCode);
            }
            return price;
        }
    }
}
