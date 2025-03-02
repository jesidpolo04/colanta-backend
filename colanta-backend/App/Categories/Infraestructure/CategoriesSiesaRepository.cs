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
    using System;

    public class HttpCategoriesSiesaRepository : ICategoriesSiesaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HttpCategoriesSiesaRepository> _logger;
        private  readonly HttpClient _httpClient;
        private readonly SiesaAuth _siesaAuth;
        public HttpCategoriesSiesaRepository(IConfiguration configuration, ILogger<HttpCategoriesSiesaRepository> logger)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _siesaAuth = new SiesaAuth(configuration);
            _logger = logger;
        }

        public async Task<Category[]> GetAllCategories()
        {
            string endpoint = "/api/ColantaWS/FamiliasLineas";
            string uri = _configuration["SiesaUrl"] + endpoint;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _siesaAuth.getToken());
            HttpResponseMessage siesaResponse = await _httpClient.SendAsync(request);
            string siesaBodyResponse = await siesaResponse.Content.ReadAsStringAsync();
            if (!siesaResponse.IsSuccessStatusCode)
            {
                _logger.LogError("Siesa respondió con status: {StatusCode}", siesaResponse.StatusCode);
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            SiesaCategoriesDto siesaCategoriesDto = JsonSerializer.Deserialize<SiesaCategoriesDto>(siesaBodyResponse);
            List<Category> categories = new List<Category>();
            foreach(SiesaCategoryDto siesaCategoryDto in siesaCategoriesDto.Familias)
            {
                try
                {
                    categories.Add(siesaCategoryDto.ToCategory());
                }
                catch(Exception exception)
                {
                    _logger.LogError(
                        exception, 
                        "Error al convertir el dto proviniente de siesa en una categoría, message: {Message}, stack: {Stack}", 
                        exception.Message,
                        exception.StackTrace
                    );
                }
            }
            return categories.ToArray();
        }
    }
}
