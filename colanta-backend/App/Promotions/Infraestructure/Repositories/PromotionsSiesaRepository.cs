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
    using Microsoft.Extensions.Configuration;
    public class PromotionsSiesaRepository : Domain.PromotionsSiesaRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;

        public PromotionsSiesaRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }
        public async Task<Promotion[]> getAllPromotions()
        {
            string endpoint = "/promociones";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(400, "Hubo un problema al consultar las promociones en Siesa, respondió con status" + siesaResponse.StatusCode);
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaPromotionsDto siesaPromotionsDto = JsonSerializer.Deserialize<SiesaPromotionsDto>(siesaResponseBody);
            List<Promotion> promotions = new List<Promotion>();
            foreach(SiesaPromotionDto siesaPromotionDto in siesaPromotionsDto.promociones)
            {
                promotions.Add(siesaPromotionDto.getPromotionFromDto());
            }
            return promotions.ToArray();
        }
    }
}
