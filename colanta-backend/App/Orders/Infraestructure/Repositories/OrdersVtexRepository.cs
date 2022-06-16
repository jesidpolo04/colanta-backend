namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using App.Shared.Domain;
    using colanta_backend.App.Orders.Domain;

    public class OrdersVtexRepository : Domain.OrdersVtexRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;
        public OrdersVtexRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnvironment = configuration["MercolantaEnvironment"];

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);
        }

        public async Task<string> getOrderByVtexId(string vtexOrderId)
        {
            string endpoint = "/api/oms/pvt/orders/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexOrderId;
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("No fue posible consultar la orden con Vtex Id" + vtexOrderId + " Vtex respondió con status: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            return vtexResponseBody;
        }
    }
}
