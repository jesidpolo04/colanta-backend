namespace colanta_backend.App.Brands.Infraestructure
{
    using App.Brands.Domain;
    using App.Shared.Domain;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public class VtexBrandsRepository : BrandsVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        private string apiToken;
        private string apiKey;
        private string accountName;
        private string vtexEnviroment;
        public VtexBrandsRepository(IConfiguration configuration)
        {
            this.configuration = configuration;

            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnviroment = configuration["MercolantaEnvironment"];
            
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

        public void changeEnviroment(string enviroment)
        {
            enviroment = enviroment.Trim();
            string[] possibleValues = { "mercolanta", "agrocolanta" };

            foreach(string possibleValue in possibleValues)
            {
                if (enviroment == possibleValue)
                {
                    if(possibleValue == "mercolanta")
                    {
                        this.apiKey = configuration["MercolantaVtexApiKey"];
                        this.apiToken = configuration["MercolantaVtexToken"];
                        this.accountName = configuration["MercolantaAccountName"];
                        this.vtexEnviroment = configuration["MercolantaEnvironment"];
                    }
                    if(possibleValue == "agrocolanta")
                    {
                        this.apiKey = configuration["AgrocolantaVtexApiKey"];
                        this.apiToken = configuration["AgrocolantaVtexToken"];
                        this.accountName = configuration["AgrocolantaAccountName"];
                        this.vtexEnviroment = configuration["AgrocolantaEnvironment"];
                    }
                    this.setCredentialHeaders();
                    return;
                }
            }
            throw new ArgumentOutOfRangeException(paramName: "enviroment", message: "Invalid Enviroment, Only can be: 'mercolanta' or 'agrocolanta'");
        }

        public Task<Brand[]> getAllBrands()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Brand> getBrandByVtexId(int? id)
        {
            if (id == null)
            {
                throw new VtexException("El id vtex es nulo, no se puede realizar la consulta con un id nulo");
            }
            string requestEndpoint = "https://" + this.accountName + "." + this.vtexEnviroment + "/api/catalog_system/pvt/brand/" + id.ToString();
            HttpResponseMessage responseVtex = await this.httpClient.GetAsync(requestEndpoint);
            if (!responseVtex.IsSuccessStatusCode)
            {
                throw new VtexException("VTEX repondió con status: " + responseVtex.StatusCode + " al intentar obtener la marca con id vtex: " + id);
            }
            string responseBodyVtex = await responseVtex.Content.ReadAsStringAsync();
            GetVtexBrandDTO vtexBrand = JsonSerializer.Deserialize<GetVtexBrandDTO>(responseBodyVtex);
            return vtexBrand.toBrand();
        }

        public async Task<Brand?> saveBrand(Brand brand)
        {
            string url = "https://" + this.accountName + "." + this.vtexEnviroment;
            string endpoint = "/api/catalog/pvt/brand";
            string jsonContent = JsonSerializer.Serialize(new
            {
                Name = brand.name,
                Active = brand.state,
                SiteTitle = brand.name,
                Text = "",
                Keywords = "",
                MenuHome = false
            });
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseVtex = await this.httpClient.PostAsync(url + endpoint, content);
            if (!responseVtex.IsSuccessStatusCode)
            {
                throw new VtexException("VTEX repondió con status: "+responseVtex.StatusCode+" al intentar crear la marca: "+brand.name);
            }
            string responseBodyVtex = await responseVtex.Content.ReadAsStringAsync();
            VtexBrandDTO vtexBrand = JsonSerializer.Deserialize<VtexBrandDTO>(responseBodyVtex);
            brand.id_vtex = vtexBrand.Id;
            return brand;
        }

        public async Task<Brand> updateBrand(Brand brand)
        {
            string url = "https://" + this.accountName + "." + this.vtexEnviroment;
            string endpoint = "/api/catalog/pvt/brand/" + brand.id_vtex;
            string jsonContent = JsonSerializer.Serialize(new
            {
                Name = brand.name,
                Active = brand.state,
                SiteTitle = brand.name,
                Text = "",
                Keywords = "",
                MenuHome = false
            });
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseVtex = await this.httpClient.PutAsync(url + endpoint, content);
            if (!responseVtex.IsSuccessStatusCode)
            {
                throw new VtexException("VTEX repondió con status: " + responseVtex.StatusCode + " al intentar actualizar la marca: " + brand.name);
            }
            return brand;
        }
    }
}
