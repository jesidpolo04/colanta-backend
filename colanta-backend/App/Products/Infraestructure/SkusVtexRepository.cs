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
    public class SkusVtexRepository : Domain.SkusVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;

        public SkusVtexRepository(IConfiguration configuration)
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

        public async Task<Sku> getSkuBySiesaId(string siesaId)
        {
            string endpoint = "/api/catalog/pvt/stockkeepingunit?refId=";
            string refId = siesaId;
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + refId;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("No fue posible crear el SKU, Vtex respondió con estado: " + vtexResponse.StatusCode);
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
            VtexSkuDto skuDto = JsonSerializer.Deserialize<VtexSkuDto>(vtexResponseBody);
            return skuDto.getSkuFromDto();
        }

        public async Task<Sku> getSkuByVtexId(string vtexId)
        {
            string endpoint = "/api/catalog/pvt/stockkeepingunit/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexId;
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException("No fue posible crear el SKU, Vtex respondió con estado: " + vtexResponse.StatusCode);
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
            VtexSkuDto skuDto = JsonSerializer.Deserialize<VtexSkuDto>(vtexResponseBody);
            return skuDto.getSkuFromDto();
        }

        public async Task<Sku> saveSku(Sku sku)
        {
            string endpoint = "/api/catalog/pvt/stockkeepingunit";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            CreateVtexSkuDto requestBody = new CreateVtexSkuDto();
            requestBody.ProductId = sku.product_id;
            requestBody.Name = sku.name;
            requestBody.RefId = sku.ref_id;
            requestBody.UnitMultiplier = sku.unit_multiplier;
            requestBody.MeasurementUnit = sku.measurement_unit;
            requestBody.IsActive = sku.is_active;
            requestBody.PackagedHeight = sku.packaged_height;
            requestBody.PackagedWidth = sku.packaged_width;
            requestBody.PackagedLength = sku.packaged_length;
            requestBody.PackagedWeightKg = sku.packaged_weight_kg;

            string jsonContent = JsonSerializer.Serialize(requestBody);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException("No fue posible crear el SKU, Vtex respondió con estado: " + vtexResponse.StatusCode);
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            CreatedVtexSkuDto skuDto = JsonSerializer.Deserialize<CreatedVtexSkuDto>(vtexResponseBody);
            return skuDto.getSkuFromDto();
        }

        public Task<Sku> updateSku(Sku sku)
        {
            throw new System.NotImplementedException();
        }
    }
}
