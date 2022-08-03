using colanta_backend.App.Promotions.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Shared.Domain;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    public class PromotionsSiesaRepository : Domain.PromotionsSiesaRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private SiesaAuth siesaAuth;

        public PromotionsSiesaRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromMinutes(5);
            this.configuration = configuration;
            this.siesaAuth = new SiesaAuth(configuration);
        }
        public async Task<Promotion[]> getAllPromotions()
        {
            await this.setHeaders();
            string endpoint = "/api/ColantaWS/Descuentos";
            
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaPromotionsDto siesaPromotionsDto = JsonSerializer.Deserialize<SiesaPromotionsDto>(siesaResponseBody);
            List<Promotion> promotions = new List<Promotion>();
            SiesaPromotionMapper promotionMapper = new SiesaPromotionMapper();
            foreach(SiesaPromotionDto siesaPromotionDto in siesaPromotionsDto.promociones)
            {
                promotions.Add(promotionMapper.getPromotionFromDto(siesaPromotionDto));
            }
            return promotions.ToArray();
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
