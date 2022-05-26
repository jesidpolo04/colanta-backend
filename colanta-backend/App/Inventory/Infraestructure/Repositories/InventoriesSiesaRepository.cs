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
    using Microsoft.Extensions.Configuration;
    public class InventoriesSiesaRepository : Domain.InventoriesSiesaRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;

        public InventoriesSiesaRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.httpClient = new HttpClient();
        }

        public async Task<Inventory[]> getAllInventoriesByWarehouse(string warehouseSiesaId)
        {
            string endpoint = "/inventario_almacen";
            HttpResponseMessage siesaResponse = await this.httpClient.GetAsync(configuration["SiesaUrl"] + endpoint);
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(400, "Hubo un problema con Siesa, al consultar los inventarios del almacen: " + warehouseSiesaId);
            }
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            SiesaInventoriesDto siesaInventoriesDto = JsonSerializer.Deserialize<SiesaInventoriesDto>(siesaResponseBody);
            List<Inventory> inventories = new List<Inventory>();
            foreach (SiesaInventoryDto siesaInventoryDto in siesaInventoriesDto.inventario) 
            {
                Inventory inventory = siesaInventoryDto.getInventoryFromDto();
                inventory.warehouse_siesa_id = warehouseSiesaId;
                inventories.Add(inventory);
            }
            return inventories.ToArray();
        }
    }
}
