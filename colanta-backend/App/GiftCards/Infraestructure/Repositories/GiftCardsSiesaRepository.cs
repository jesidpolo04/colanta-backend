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
    using Microsoft.Extensions.Configuration;

    public class GiftCardsSiesaRepository : Domain.GiftCardsSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        public GiftCardsSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }

        public async Task<GiftCard[]> getAllGiftCards()
        {
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

        public Task<string> getAuthenticationToken(string user, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<decimal> getGiftCardBalanceBySiesaId(string siesaId)
        {
            string endpoint = "/balance_tarjeta";
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
            string endpoint = "/tarjetas_documento_negocio";
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
    }
}
