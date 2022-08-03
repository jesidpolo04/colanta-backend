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
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;

    public class CategoriesMockSiesaRepository : Domain.CategoriesSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private SiesaAuth siesaAuth;
        public CategoriesMockSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
            this.siesaAuth = new SiesaAuth(configuration);
        }
        public async Task<Category[]> getAllCategories()
        {
            await this.setHeaders();
            string endpoint = "/api/ColantaWS/FamiliasLineas";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            SiesaCategoriesDto siesaCategoriesDto = JsonSerializer.Deserialize<SiesaCategoriesDto>(siesaBodyResponse);
            List<Category> categories = new List<Category>();
            foreach(SiesaCategoryDto siesaCategoryDto in siesaCategoriesDto.familias)
            {
                if(siesaCategoryDto.id == "3029_fail") //3029
                {
                    continue;
                }
                categories.Add(siesaCategoryDto.toCategory());
            }
            return categories.ToArray();
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
