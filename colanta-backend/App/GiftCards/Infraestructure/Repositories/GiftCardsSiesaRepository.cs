using colanta_backend.App.GiftCards.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.GiftCards.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using Shared.Domain;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;

    public class GiftCardsSiesaRepository : Domain.GiftCardsSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private SiesaAuth siesaAuth;

        public GiftCardsSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }

        public async Task<GiftCard[]> getAllGiftCards()
        {
            this.setHeaders().Wait();
            string endpoint = "/tarjetas";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            SiesaGiftCardsDto siesaGiftCardsDto = JsonSerializer.Deserialize<SiesaGiftCardsDto>(siesaBodyResponse);
            List<GiftCard> gifCards = new List<GiftCard>();
            foreach(SiesaGiftCardDto siesaGiftCardDto in siesaGiftCardsDto.tarjetas)
            {
                gifCards.Add(siesaGiftCardDto.getGiftCardFromDto());
            }
            return gifCards.ToArray();
        }

        public async Task<decimal> getGiftCardBalanceBySiesaId(string cardSiesaId)
        {
            this.setHeaders().Wait();
            string endpoint = $"/api/ColantaWS/tarjetas/{cardSiesaId}";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            SiesaBalanceGiftCardDto siesaBalanceGiftCardDto = JsonSerializer.Deserialize<SiesaBalanceGiftCardDto>(siesaBodyResponse);
            return siesaBalanceGiftCardDto.balance;
        }

        public async Task<GiftCard[]> getGiftCardsByDocumentAndBusiness(string document, string business)
        {
            this.setHeaders().Wait();
            string endpoint = $"/api/ColantaWS/tarjetas/documento/{document}/{business}";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            SiesaGiftCardsDto siesaGiftCardsDto = JsonSerializer.Deserialize<SiesaGiftCardsDto>(siesaBodyResponse);
            List<GiftCard> gifCards = new List<GiftCard>();
            foreach (SiesaGiftCardDto siesaGiftCardDto in siesaGiftCardsDto.tarjetas)
            {
                gifCards.Add(siesaGiftCardDto.getGiftCardFromDto());
            }
            return gifCards.ToArray();
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
