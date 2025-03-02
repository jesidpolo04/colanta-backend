namespace colanta_backend.App.Categories.Infraestructure
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Categories.Domain;
    using colanta_backend.App.Shared.Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class CategoriesVtexRepository : ICategoriesVtexRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriesVtexRepository> _logger;
        private readonly HttpClient _httpClient;

        private string _apiKey;
        private string _apiToken;
        private string _accountName;
        private string _vtexEnvironment;
        public CategoriesVtexRepository(IConfiguration configuration, ILogger<CategoriesVtexRepository> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _apiKey = configuration["MercolantaVtexApiKey"];
            _apiToken = configuration["MercolantaVtexToken"];
            _accountName = configuration["MercolantaAccountName"];
            _vtexEnvironment = configuration["MercolantaEnvironment"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://{_accountName}.{_vtexEnvironment}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void ChangeEnvironment(string environment)
        {
            environment = environment.Trim();
            string[] possibleValues = { "mercolanta", "agrocolanta" };

            foreach (string possibleValue in possibleValues)
            {
                if (environment == possibleValue)
                {
                    if (possibleValue == "mercolanta")
                    {
                        _apiKey = _configuration["MercolantaVtexApiKey"];
                        _apiToken = _configuration["MercolantaVtexToken"];
                        _accountName = _configuration["MercolantaAccountName"];
                        _vtexEnvironment = _configuration["MercolantaEnvironment"];
                    }
                    if (possibleValue == "agrocolanta")
                    {
                        _apiKey = _configuration["AgrocolantaVtexApiKey"];
                        _apiToken = _configuration["AgrocolantaVtexToken"];
                        _accountName = _configuration["AgrocolantaAccountName"];
                        _vtexEnvironment = _configuration["AgrocolantaEnvironment"];
                    }
                    return;
                }
            }
            throw new ArgumentOutOfRangeException(paramName: "enviroment", message: "Invalid Enviroment, Only can be: 'mercolanta' or 'agrocolanta'");
        }

        private void SetCredentialHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("X-VTEX-API-AppToken", _apiToken);
            request.Headers.Add("X-VTEX-API-AppKey", _apiKey);
        }

        public async Task<Category?> GetCategoryById(int vtexId)
        {
            _logger.LogInformation("Buscando en vtex la categoría con vtex id: {VtexId}", vtexId);
            string endpoint = $"/api/catalog/pvt/category/{vtexId}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            SetCredentialHeaders(request);
            HttpResponseMessage vtexResponse = await _httpClient.SendAsync(request);
            _logger.LogTrace("Vtex respondió con status {StatusCode}", vtexResponse.StatusCode);
            if (vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            _logger.LogTrace("Vtex respondió con el cuerpo {ResponseBody}", vtexResponseBody);
            VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(vtexResponseBody);
            return categoryDto.toCategory();
        }

        public async Task<Category?> GetCategoryByName(string name)
        {
            _logger.LogInformation("Buscando en vtex la categoría con nombre: {Name}", name);
            int treeLevel = 3;
            string endpoint = $"/api/catalog_system/pub/category/tree/{treeLevel}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            SetCredentialHeaders(request);
            HttpResponseMessage vtexResponse = await _httpClient.SendAsync(request);
            _logger.LogTrace("Vtex respondió con status {StatusCode}", vtexResponse.StatusCode);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            _logger.LogTrace("Vtex respondió con el cuerpo {ResponseBody}", vtexResponseBody);
            VtexTreeCategoryDto[] treeCategoriesDto = JsonSerializer.Deserialize<VtexTreeCategoryDto[]>(vtexResponseBody);
            foreach(VtexTreeCategoryDto treeCategoryDto in treeCategoriesDto)
            {
                Category family = treeCategoryDto.toCategory();
                if (family.Name == name)
                {
                    return family;
                }
                foreach(Category line in family.Childs)
                {
                    if (line.Name == name)
                    {
                        return line;
                    }
                }
            }
            return null;
        }

        public async Task<Category> GetCategoryByVtexId(int vtexId)
        {
            _logger.LogInformation("Buscando en vtex la categoría con vtex id: {VtexId}", vtexId);
            string endpoint = $"/api/catalog/pvt/category/{vtexId}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            SetCredentialHeaders(request);
            HttpResponseMessage vtexResponse = await _httpClient.SendAsync(request);
            _logger.LogTrace("Vtex respondió con status {StatusCode}", vtexResponse.StatusCode);
            if (vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            if(vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status: {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            _logger.LogTrace("Vtex respondió con el cuerpo {ResponseBody}", vtexResponseBody);
            VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(vtexResponseBody);
            return categoryDto.toCategory();
        }

        public async Task<Category> SaveCategory(Category category)
        {
            _logger.LogInformation("Guardando en vtex la categoría con nombre: {Name}", category.Name);
            string endpoint = "/api/catalog/pvt/category";
            string jsonContent;
            var MERCOLANTA_DEFAULT_CATEGORY = MercolantaCategory.defaultGlobalCategory;
            var AGROCOLANTA_DEFAULT_CATEGORY = AgrocolantaCategory.defaultGlobalCategory;
            Category existCategory = await this.GetCategoryByName(category.Name);
            if(existCategory != null)
            {
                _logger.LogWarning("La categoría con nombre {Name} ya existe en vtex", category.Name);
                return existCategory;
            }

            if(category.Father != null)
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.Name,
                    IsActive = category.IsActive,
                    Title = category.Name,
                    FatherCategoryId = category.Father.VtexId,
                    GlobalCategoryId = category.Business == "mercolanta" ? MERCOLANTA_DEFAULT_CATEGORY : AGROCOLANTA_DEFAULT_CATEGORY,
                });
            }
            else
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.Name,
                    IsActive = category.IsActive,
                    Title = category.Name,
                    FatherCategoryId = category.Business == "mercolanta" ? MercolantaCategory.vtexId : AgrocolantaCategory.vtexId,
                    GlobalCategoryId = category.Business == "mercolanta" ? MERCOLANTA_DEFAULT_CATEGORY : AGROCOLANTA_DEFAULT_CATEGORY,
                }) ;
            }
            _logger.LogTrace("El cuerpo de la petición es {JsonContent}", jsonContent);
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            SetCredentialHeaders(request);
            request.Content = content;
            HttpResponseMessage vtexResponse = await this._httpClient.SendAsync(request);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexBodyResponse = await vtexResponse.Content.ReadAsStringAsync();
            _logger.LogTrace("Vtex respondió con el cuerpo {ResponseBody}", vtexBodyResponse);
            CreatedVtexCategoryDto createdCategoryDto = JsonSerializer.Deserialize<CreatedVtexCategoryDto>(vtexBodyResponse);
            Category createdCategory = createdCategoryDto.toCategory();
            category.VtexId = createdCategory.VtexId;
            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _logger.LogInformation("Actualizando en vtex la categoría con vtex id: {VtexId} nombre: {Nombre}", category.VtexId, category.Name);
            int? vtexId = category.VtexId;
            string endpoint = $"/api/catalog/pvt/category/{vtexId}";
            string jsonContent;
            if (category.Father != null)
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.Name,
                    IsActive = category.IsActive,
                    Title = category.Name,
                    FatherCategoryId = category.Father.VtexId
                });
            }
            else
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.Name,
                    IsActive = category.IsActive,
                    Title = category.Name,
                });
            }
            _logger.LogTrace("El cuerpo de la petición es {JsonContent}", jsonContent);
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, endpoint);
            SetCredentialHeaders(request);
            request.Content = content;
            HttpResponseMessage vtexResponse = await this._httpClient.SendAsync(request);
            _logger.LogTrace("Vtex respondió con status {StatusCode}", vtexResponse.StatusCode);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            return category;
        }

        public async Task<bool> UpdateCategoryState(int vtexId, bool state)
        {
            try{
                _logger.LogInformation("Actualizando en vtex la categoría con vtex id: {VtexId} estado: {State}", vtexId, state);
                string endpoint = $"/api/catalog/pvt/category/{vtexId}";
                string updateEndpoint = $"/api/catalog/pvt/category/{vtexId}";
                HttpRequestMessage getRequest = new HttpRequestMessage(HttpMethod.Get, endpoint);
                SetCredentialHeaders(getRequest);
                HttpResponseMessage getCategoryResponse = await this._httpClient.SendAsync(getRequest);
                if (!getCategoryResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Vtex respondió con Status {StatusCode}", getCategoryResponse.StatusCode);
                }
                string getCategoryResponseBody = await getCategoryResponse.Content.ReadAsStringAsync();
                VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(getCategoryResponseBody);
                categoryDto.IsActive = state;
                HttpRequestMessage updateRequest = new HttpRequestMessage(HttpMethod.Put, updateEndpoint);
                SetCredentialHeaders(updateRequest);
                updateRequest.Content = new StringContent(JsonSerializer.Serialize(categoryDto), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage updateCategoryResponse = await this._httpClient.SendAsync(updateRequest);
                if (!updateCategoryResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Vtex respondió con Status {StatusCode}", updateCategoryResponse.StatusCode);
                    return false;
                }
                return true;
            }catch(Exception exception){
                _logger.LogError(exception, "Error actualizando el estado de la categoría en Vtex: {Message}", exception.Message);
                return false;
            }
        }

        public async Task<bool> UpdateCategoryFather(int vtexId, int fatherVtexId)
        {
            _logger.LogInformation("Actualizando en vtex el padre de la categoría con vtex id: {VtexId}, padre vtex id: {FatherVtexId}", vtexId, fatherVtexId);
            string getCategoryEndpoint = $"/api/catalog/pvt/category/{vtexId}";
            string updateCategoryEndpoint = $"/api/catalog/pvt/category/{vtexId}";
            HttpRequestMessage getRequest = new HttpRequestMessage(HttpMethod.Get, getCategoryEndpoint);
            SetCredentialHeaders(getRequest);
            HttpResponseMessage getCategoryResponse = await this._httpClient.SendAsync(getRequest);
            _logger.LogTrace("Vtex respondió con status {StatusCode} al obtener la categoría", getCategoryResponse.StatusCode);
            if (!getCategoryResponse.IsSuccessStatusCode)
            {
                throw new VtexException(getCategoryResponse, $"Vtex respondió con Status {getCategoryResponse.StatusCode}");
            }
            string getCategoryResponseBody = await getCategoryResponse.Content.ReadAsStringAsync();
            VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(getCategoryResponseBody);
            categoryDto.FatherCategoryId = fatherVtexId;
            HttpContent requestBody = new StringContent(JsonSerializer.Serialize(categoryDto), System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage updateRequest = new HttpRequestMessage(HttpMethod.Put, updateCategoryEndpoint);
            SetCredentialHeaders(updateRequest);
            updateRequest.Content = requestBody;
            HttpResponseMessage updateCategoryResponse = await this._httpClient.SendAsync(updateRequest);
            _logger.LogTrace("Vtex respondió con status {StatusCode} al actualizar la categoría", updateCategoryResponse.StatusCode);
            if (!updateCategoryResponse.IsSuccessStatusCode)
            {
                throw new VtexException(updateCategoryResponse, $"Vtex respondió con Status {updateCategoryResponse.StatusCode}");
            }
            return true;
        }
    }
}
