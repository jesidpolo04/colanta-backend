namespace colanta_backend.App.Categories.Infraestructure
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Categories.Domain;
    using Shared.Domain;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class CategoriesMockSiesaRepository : CategoriesSiesaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriesMockSiesaRepository> _logger;
        private  readonly HttpClient _httpClient;
        private readonly SiesaAuth _siesaAuth;
        public CategoriesMockSiesaRepository(IConfiguration configuration, ILogger<CategoriesMockSiesaRepository> logger)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _siesaAuth = new SiesaAuth(configuration);
            _logger = logger;
        }

        public async Task<Category[]> getAllCategories()
        {
            await this.setHeaders();
            string endpoint = "/api/ColantaWS/FamiliasLineas";
            HttpResponseMessage siesaResponse = await _httpClient.GetAsync(_configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            SiesaCategoriesDto siesaCategoriesDto = JsonSerializer.Deserialize<SiesaCategoriesDto>(siesaBodyResponse);
            List<Category> categories = new List<Category>();
            foreach(SiesaCategoryDto siesaCategoryDto in siesaCategoriesDto.familias)
            {
                categories.Add(siesaCategoryDto.toCategory());
            }
            return categories.ToArray();
        }

        private async Task setHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _siesaAuth.getToken());
        }
    }
}
