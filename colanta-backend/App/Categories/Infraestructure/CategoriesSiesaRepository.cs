namespace colanta_backend.App.Categories.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Categories.Domain;
    using Shared.Domain;
    using Microsoft.Extensions.Configuration;

    public class CategoriesMockSiesaRepository : Domain.CategoriesSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        public CategoriesMockSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }
        public async Task<Category[]> getAllCategories()
        {
            string endpoint = "/familias";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(((int)siesaResponse.StatusCode), "Falló la petición a siesa con estado: " + siesaResponse.StatusCode);
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            MockSiesaCategoryDto[] mockSiesaCateoriesDto = JsonSerializer.Deserialize<MockSiesaCategoryDto[]>(siesaBodyResponse);
            List<Category> categories = new List<Category>();
            foreach(MockSiesaCategoryDto mockSiesaCategoryDto in mockSiesaCateoriesDto)
            {
                categories.Add(mockSiesaCategoryDto.toCategory());
            }
            return categories.ToArray();
        }
    }
}
