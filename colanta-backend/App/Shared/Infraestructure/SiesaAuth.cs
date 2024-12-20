﻿namespace colanta_backend.App.Shared.Infraestructure
{

    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using App.Brands.Domain;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json.Linq;
    using Shared.Application;
    using Shared.Domain;

    public class SiesaAuth
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private string siesaUrl;

        public SiesaAuth(IConfiguration configuration, HttpSSLHandler handler)
        {
            this.httpClient = new HttpClient(handler);
            this.configuration = configuration;
            this.siesaUrl = this.configuration["SiesaUrl"];
        }

        public async Task<string> getToken()
        {
            string user = this.configuration["SiesaUser"];
            string password = this.configuration["SiesaPassword"];
            string endpoint = "/api/Login/Login";
            string url = this.siesaUrl + endpoint;

            object requestBody = new
            {
                username = user,
                password = password,
            };
            string jsonRequestBody = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage siesaResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string stringResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            JObject responseBody = JObject.Parse(stringResponseBody);
            string token = (string)responseBody["token"];
            return token;
        }
    }
}