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

    public class CategoriesVtexRepository : Domain.CategoriesVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;
        public CategoriesVtexRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnvironment = configuration["MercolantaEnvironment"];

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.setCredentialHeaders();
        }

        public void changeEnvironment(string environment)
        {
            environment = environment.Trim();
            string[] possibleValues = { "mercolanta", "agrocolanta" };

            foreach (string possibleValue in possibleValues)
            {
                if (environment == possibleValue)
                {
                    if (possibleValue == "mercolanta")
                    {
                        this.apiKey = configuration["MercolantaVtexApiKey"];
                        this.apiToken = configuration["MercolantaVtexToken"];
                        this.accountName = configuration["MercolantaAccountName"];
                        this.vtexEnvironment = configuration["MercolantaEnvironment"];
                    }
                    if (possibleValue == "agrocolanta")
                    {
                        this.apiKey = configuration["AgrocolantaVtexApiKey"];
                        this.apiToken = configuration["AgrocolantaVtexToken"];
                        this.accountName = configuration["AgrocolantaAccountName"];
                        this.vtexEnvironment = configuration["AgrocolantaEnvironment"];
                    }
                    this.setCredentialHeaders();
                    return;
                }
            }
            throw new ArgumentOutOfRangeException(paramName: "enviroment", message: "Invalid Enviroment, Only can be: 'mercolanta' or 'agrocolanta'");
        }

        private void setCredentialHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppToken");
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppKey");

            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);
        }

        public async Task<Category?> getCategoryById(int vtexId)
        {
            string endpoint = "/api/catalog/pvt/category/";
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync("https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexId);
            if (vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(vtexResponseBody);
            return categoryDto.toCategory();
        }

        public async Task<Category?> getCategoryByName(string name)
        {
            string endpoint = "/api/catalog_system/pub/category/tree/";
            int treeLevel = 3;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync("https://" + this.accountName + "." + this.vtexEnvironment + endpoint + treeLevel);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexTreeCategoryDto[] treeCategoriesDto = JsonSerializer.Deserialize<VtexTreeCategoryDto[]>(vtexResponseBody);
            foreach(VtexTreeCategoryDto treeCategoryDto in treeCategoriesDto)
            {
                Category family = treeCategoryDto.toCategory();
                if (family.name == name)
                {
                    return family;
                }
                foreach(Category line in family.childs)
                {
                    if (line.name == name)
                    {
                        return line;
                    }
                }
            }
            return null;
        }

        public async Task<Category> getCategoryByVtexId(int vtexId)
        {
            string endpoint = "/api/catalog/pvt/category/";
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync("https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexId);
            if (vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(vtexResponseBody);
            return categoryDto.toCategory();
        }

        public async Task<Category> saveCategory(Category category)
        {
            string endpoint = "/api/catalog/pvt/category";
            string jsonContent;
            var MERCOLANTA_DEFAULT_CATEGORY = MercolantaCategory.defaultGlobalCategory;
            var AGROCOLANTA_DEFAULT_CATEGORY = AgrocolantaCategory.defaultGlobalCategory;

            Category existCategory = await this.getCategoryByName(category.name);
            if(existCategory != null)
            {
                return existCategory;
            }

            if(category.father != null)
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.name,
                    IsActive = category.isActive,
                    Title = category.name,
                    FatherCategoryId = category.father.vtex_id,
                    GlobalCategoryId = category.business == "mercolanta" ? MERCOLANTA_DEFAULT_CATEGORY : AGROCOLANTA_DEFAULT_CATEGORY,
                });
            }
            else
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.name,
                    IsActive = category.isActive,
                    Title = category.name,
                    FatherCategoryId = category.business == "mercolanta" ? MercolantaCategory.vtexId : AgrocolantaCategory.vtexId,
                    GlobalCategoryId = category.business == "mercolanta" ? MERCOLANTA_DEFAULT_CATEGORY : AGROCOLANTA_DEFAULT_CATEGORY,
                }) ;
            }
            
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync("https://" + this.accountName + "." + this.vtexEnvironment + endpoint, content);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexBodyResponse = await vtexResponse.Content.ReadAsStringAsync();
            CreatedVtexCategoryDto createdCategoryDto = JsonSerializer.Deserialize<CreatedVtexCategoryDto>(vtexBodyResponse);
            Category createdCategory = createdCategoryDto.toCategory();
            category.vtex_id = createdCategory.vtex_id;
            return category;
        }

        public async Task<Category> updateCategory(Category category)
        {
            string endpoint = "/api/catalog/pvt/category/";
            int? vtexId = category.vtex_id;
            string jsonContent;
            if (category.father != null)
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.name,
                    IsActive = category.isActive,
                    Title = category.name,
                    FatherCategoryId = category.father.vtex_id
                });
            }
            else
            {
                jsonContent = JsonSerializer.Serialize(new
                {
                    Name = category.name,
                    IsActive = category.isActive,
                    Title = category.name,
                });
            }
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PutAsync("https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexId, content);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            return category;
        }

        public async Task<bool> updateCategoryState(int vtexId, bool state)
        {
            string getCategoryEndpoint = $"https://{this.accountName}.{this.vtexEnvironment}/api/catalog/pvt/category/{vtexId}";
            HttpResponseMessage getCategoryResponse = await this.httpClient.GetAsync(getCategoryEndpoint);
            if (getCategoryResponse.IsSuccessStatusCode != true)
            {
                throw new VtexException(getCategoryResponse, $"Vtex respondió con Status {getCategoryResponse.StatusCode}");
            }
            string getCategoryResponseBody = await getCategoryResponse.Content.ReadAsStringAsync();
            VtexCategoryDto categoryDto = JsonSerializer.Deserialize<VtexCategoryDto>(getCategoryResponseBody);
            categoryDto.IsActive = state;
            string updateCategoryEndpoint = $"https://{this.accountName}.{this.vtexEnvironment}/api/catalog/pvt/category/{vtexId}";
            HttpContent requestBody = new StringContent(JsonSerializer.Serialize(categoryDto), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage updateCategoryResponse = await this.httpClient.PutAsync(updateCategoryEndpoint, requestBody);
            if (updateCategoryResponse.IsSuccessStatusCode != true)
            {
                throw new VtexException(updateCategoryResponse, $"Vtex respondió con Status {updateCategoryResponse.StatusCode}");
            }
            return true;
        }
    }
}
