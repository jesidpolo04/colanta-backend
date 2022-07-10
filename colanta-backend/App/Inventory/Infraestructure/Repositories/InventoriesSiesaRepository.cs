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
            System.Console.WriteLine("warehouse id: " + warehouseSiesaId);
            await this.setHeaders();
            string endpoint = "/api/ColantaWS/Inventario/" + warehouseSiesaId;
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(400, "Hubo un problema con Siesa, al consultar los inventarios del almacen: " + warehouseSiesaId);
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

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
