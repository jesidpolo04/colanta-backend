using System.Threading.Tasks;

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
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    public class InventoriesSiesaRepository : Domain.InventoriesSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private SiesaAuth siesaAuth;

        public InventoriesSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
            this.siesaAuth = new SiesaAuth(configuration);
        }

        public async Task<Inventory[]> getAllInventoriesByWarehouse(string warehouseSiesaId)
        {
            Console.WriteLine("warehouse id: " + warehouseSiesaId);
            string token = await this.siesaAuth.getToken();
            string endpoint = "/api/ColantaWS/Inventario/" + warehouseSiesaId;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, configuration["SiesaUrl"] + endpoint);
            request.Headers.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage siesaResponse = await this.httpClient.SendAsync(request);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaInventoriesDto siesaInventoriesDto = JsonSerializer.Deserialize<SiesaInventoriesDto>(siesaResponseBody);
            List<Inventory> inventories = new List<Inventory>();
            foreach (SiesaInventoryDto siesaInventoryDto in siesaInventoriesDto.inventario_almacen) 
            {
                Inventory inventory = siesaInventoryDto.getInventoryFromDto();
                inventory.warehouse_siesa_id = warehouseSiesaId;
                inventories.Add(inventory);
            }
            return inventories.ToArray();
        }
    }
}
