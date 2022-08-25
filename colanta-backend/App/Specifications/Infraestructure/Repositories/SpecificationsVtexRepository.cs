namespace colanta_backend.App.Specifications.Infraestructure
{
    using colanta_backend.App.Specifications.Domain;
    using Specifications.Domain;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Text.Json;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using System.Net.Http;
    using Shared.Domain;

    public class SpecificationsVtexRepository : VtexRepository , Domain.SpecificationsVtexRepository
    {
        public SpecificationsVtexRepository(IConfiguration configuration) :base(configuration)
        {
        }
        public async Task<List<Specification>> getProductSpecifications(int productVtexId)
        {
            string endpoint = $"/api/catalog_system/pvt/products/{productVtexId}/specification";
            string url = $"https://{this.accountName}.{this.vtexEnvironment}{endpoint}";
            HttpResponseMessage vtexResponse = await this.httpClient.GetAsync(url);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status: {vtexResponse.StatusCode}");
            }
            string responseBody = await vtexResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Specification>>(responseBody);
        }

        public async Task<Specification> updateProductSpecification(int productVtexId, Specification specification)
        {
            string endpoint = $"/api/catalog_system/pvt/products/{productVtexId}/specification";
            string url = $"https://{this.accountName}.{this.vtexEnvironment}{endpoint}";
            List<Specification> specifications = new List<Specification>();
            specifications.Add(specification);
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(specifications), 
                System.Text.Encoding.UTF8,
                "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status: {vtexResponse.StatusCode}");
            }
            return specification;
        }
    }
}
