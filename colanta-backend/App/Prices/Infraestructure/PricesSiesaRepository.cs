using colanta_backend.App.Prices.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Prices.Infraestructure
{
    using Prices.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Shared.Domain;
    using Microsoft.Extensions.Configuration;
    public class PricesSiesaRepository : Domain.PricesSiesaRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        public PricesSiesaRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }
        public async Task<Price[]> getAllPrices()
        {
            string endpoint = "/precios";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(400, "Hubo un problema al consultar los precios en Siesa, respondió con status" + siesaResponse.StatusCode);
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaPricesDto siesaPricesDto = JsonSerializer.Deserialize<SiesaPricesDto>(siesaResponseBody);
            List<Price> prices = new List<Price>();
            foreach(SiesaPriceDto siesaPriceDto in siesaPricesDto.precios_productos)
            {
                prices.Add(siesaPriceDto.GetPriceFromDto());
            }
            return prices.ToArray();
        }
    }
}
