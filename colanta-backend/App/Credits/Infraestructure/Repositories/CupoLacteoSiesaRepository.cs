namespace colanta_backend.App.Credits.Infraestructure.Repositories
{
    using Promotions.Domain;
    using Credits.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using Shared.Domain;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using colanta_backend.App.GiftCards.Domain;

    public class CupoLacteoSiesaRepository : Domain.CupoLacteoSiesaRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private SiesaAuth siesaAuth;

        public CupoLacteoSiesaRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromMinutes(5);
            this.configuration = configuration;
            this.siesaAuth = new SiesaAuth(configuration);
        }

        public async Task<GiftCard> getCupoLacteo(string document, string email, string business)
        {
            this.setHeaders().Wait();
            string endpoint = "/api/ColantaWS/CodigosCupos";
            string url = configuration["SiesaUrl"] + endpoint;
            GetCupoLacteoRequest request = new GetCupoLacteoRequest();
            request.documento = document;
            request.email = email;
            request.negocio = business;
            HttpContent requestBody = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage siesaResponse = await this.httpClient.PostAsync(url, requestBody);
            if(siesaResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return null;
            }
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string responseBody = await siesaResponse.Content.ReadAsStringAsync();
            GetCupoLacteoResponse response = JsonSerializer.Deserialize<GetCupoLacteoResponse>(responseBody);
            GiftCard card = response.getGiftcard();
            card.owner = document;
            card.business = business;
            card.owner_email = email;
            return card;
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }

    }
}
