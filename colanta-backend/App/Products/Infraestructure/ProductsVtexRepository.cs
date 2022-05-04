using colanta_backend.App.Products.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Products.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Products.Domain;
    using Shared.Domain;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    public class ProductsVtexRepository : Domain.ProductsVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;

        public ProductsVtexRepository(IConfiguration configuration)
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

        private void setCredentialHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppToken");
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppKey");

            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);
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

        public async Task<Product> getProductBySiesaId(string siesaId)
        {
            string endpoint = "/api/catalog_system/pvt/products/productgetbyrefid/";
            string refId = siesaId;
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + refId;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if(vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("Hubo un problema con Vtex al intentar buscar el producto con referencia id: " + refId);
            }
            if (vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            if (vtexResponseBody == "null")
            {
                return null;
            }
            ByRefIdVtexProductDto productDto = JsonSerializer.Deserialize<ByRefIdVtexProductDto>(vtexResponseBody);
            return productDto.getProductFromDto();
        }

        public async Task<Product> getProductByVtexId(string vtexId)
        {
            string endpoint = "api/catalog/pvt/product/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexId;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("Hubo un problema con Vtex al intentar buscar el producto con Vtex Id: " + vtexId);
            }
            if (vtexResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            if (vtexResponseBody == "null")
            {
                return null;
            }
            ByRefIdVtexProductDto productDto = JsonSerializer.Deserialize<ByRefIdVtexProductDto>(vtexResponseBody);
            return productDto.getProductFromDto();
        }

        public async Task<Product> saveProduct(Product product)
        {
            string endpoint = "/api/catalog/pvt/product";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            CreateVtexProductDto requestBody = new CreateVtexProductDto();
            requestBody.BrandId = product.brand.id_vtex;
            requestBody.CategoryId = product.category.vtex_id;
            requestBody.RefId = product.ref_id;
            requestBody.Name = product.name;
            requestBody.Description = product.description;
            requestBody.IsActive = product.is_active;
            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("No fue posible crear el producto, Vtex respondió con estado: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            CreatedVtexProductDto productDto = JsonSerializer.Deserialize<CreatedVtexProductDto>(vtexResponseBody);
            return productDto.getProductFromDto();
        }

        public Task<Product> updateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}
