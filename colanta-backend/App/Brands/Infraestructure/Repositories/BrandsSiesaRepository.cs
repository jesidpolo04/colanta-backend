namespace colanta_backend.App.Brands.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using App.Brands.Domain;

    public class BrandsSiesaRepository : IBrandsSiesaRepository
    {
        private HttpClient httpClient;
        private SiesaBrandMapper siesaBrandMapper;
        public BrandsSiesaRepository()
        {
            this.httpClient = new HttpClient();
            this.siesaBrandMapper = new SiesaBrandMapper();
        }
        public async Task<Brand[]?> getAllBrands()
        {
            HttpResponseMessage responseSiesaBrands = await this.httpClient.GetAsync("http://localhost:3000/marcas");
            if (!responseSiesaBrands.IsSuccessStatusCode)
            {
                return null;
            }
            string siesaBrandsBody = await responseSiesaBrands.Content.ReadAsStringAsync();
            List<SiesaBrandDTO> siesaBrandsDtos = JsonSerializer.Deserialize<List<SiesaBrandDTO>>(siesaBrandsBody);
            List<Brand> brands = new List<Brand>();
            foreach (SiesaBrandDTO siesaBrandDTO in siesaBrandsDtos)
            {
                brands.Add(this.siesaBrandMapper.DtoToEntity(siesaBrandDTO));
            }
            return brands.ToArray();
        }
    }
}
