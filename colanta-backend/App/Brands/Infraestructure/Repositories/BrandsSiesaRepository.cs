namespace colanta_backend.App.Brands.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using App.Brands.Domain;
    using Microsoft.Extensions.Configuration;
    using App.Shared.Infraestructure;
    using Shared.Domain;

    public class BrandsSiesaRepository : IBrandsSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private SiesaBrandMapper siesaBrandMapper;
        private SiesaAuth siesaAuth;
        public BrandsSiesaRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.siesaBrandMapper = new SiesaBrandMapper();
            this.configuration = configuration;
            this.siesaAuth = new SiesaAuth(configuration);
        }
        public async Task<Brand[]?> getAllBrands()
        {
            await this.setHeaders();
            string endpoint = "/api/ColantaWS/Marcas";
            HttpResponseMessage responseSiesaBrands = await this.httpClient.GetAsync(this.configuration["SiesaUrl"] + endpoint);
            if (!responseSiesaBrands.IsSuccessStatusCode)
            {
                throw new SiesaException(responseSiesaBrands, $"Siesa respondió con status: {responseSiesaBrands.StatusCode}");
            }
            string siesaBrandsBody = await responseSiesaBrands.Content.ReadAsStringAsync();
            SiesaBrandsDTO siesaBrandsDto = JsonSerializer.Deserialize<SiesaBrandsDTO>(siesaBrandsBody);
            List<Brand> brands = new List<Brand>();
            foreach (SiesaBrandDTO siesaBrandDTO in siesaBrandsDto.marcas)
            {
                if (siesaBrandDTO.id == "C008_failed") //C008
                {
                    continue;
                }
                brands.Add(this.siesaBrandMapper.DtoToEntity(siesaBrandDTO));
            }
            
            return brands.ToArray();
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
