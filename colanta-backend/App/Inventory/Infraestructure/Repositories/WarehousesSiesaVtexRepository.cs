namespace colanta_backend.App.Inventory.Infraestructure
{
    using Inventory.Domain;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Shared.Domain;
    using Microsoft.Extensions.Configuration;

    public class WarehousesSiesaVtexRepository : Domain.WarehousesSiesaVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        public WarehousesSiesaVtexRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }

        public async Task<Warehouse[]> getAllWarehouses()
        {
            string endpoint = "/almacenes";
            string url = configuration["SiesaUrl"] + endpoint;

            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(url);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            WarehouseDto[] warehousesDto = JsonSerializer.Deserialize<WarehouseDto[]>(siesaResponseBody);
            List<Warehouse> warehouses = new List<Warehouse>();
            foreach(WarehouseDto warehouseDto in warehousesDto)
            {
                warehouses.Add(warehouseDto.getWarehouseFromDto());
            }
            return warehouses.ToArray();
        }
    }
}
