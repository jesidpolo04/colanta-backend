namespace colanta_backend.App.Brands.Infraestructure
{
    using App.Brands.Domain;
    using App.Shared.Domain;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System;
    using Shared.Application;

    public class VtexBrandsRepository : BrandsVtexRepository
    {
        private HttpClient httpClient;
        private string url = "https://colanta.myvtex.com/";
        private string endpoint = "api/catalog/pvt/brand";
        private string apiToken = "FIROYQZUHUNDYJAEFJXKOHXRTUNTNSERIPTKGTSVGVFFVCNJOSSHOIOYLAUECFHPPWUIQQXLRCDTCSWRGDEUZXCABUYGYOSBNPPYHETYVHQMEUWSEDXZAMQSUUHWRRMD"; //se borra la D final
        private string apiKey = "vtexappkey-colanta-CNANOI";
        public VtexBrandsRepository()
        {
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);
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
            string requestEndpoint = this.url + "api/catalog_system/pvt/brand/" + id.ToString();
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
            HttpResponseMessage responseVtex = await this.httpClient.PostAsync(this.url + this.endpoint, content);
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
            string endpoint = "api/catalog/pvt/brand/" + brand.id_vtex;
            string jsonContent = JsonSerializer.Serialize(new
            {
                Name = brand.name,
                Active = brand.state
            });
            HttpContent content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage responseVtex = await this.httpClient.PutAsync(this.url + endpoint, content);
            if (!responseVtex.IsSuccessStatusCode)
            {
                throw new VtexException("VTEX repondió con status: " + responseVtex.StatusCode + " al intentar actualizar la marca: " + brand.name);
            }
            return brand;
        }
    }
}
