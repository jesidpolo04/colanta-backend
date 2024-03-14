using System.Collections.Generic;
using colanta_backend.App.Products.Domain;
using colanta_backend.App.Shared;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace colanta_backend.App.Bags
{
    public class BagsService : ErpService
    {
        private readonly SkusRepository SkusRepository;
        public BagsService(IConfiguration configuration, SkusRepository skusRepository) : base(configuration)
        {
            SkusRepository = skusRepository;
        }

        public List<Bag> GetAvailableBags()
        {
            SetHeaders();
            var url = $"{BaseUrl}/api/ColantaWS/Bolsa";
            var response = HttpClient.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"El erp respondió con status code: {response.StatusCode}");
            }
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var erpBags = JsonConvert.DeserializeObject<List<ErpBag>>(responseBody);
            var bags = new List<Bag>();
            foreach (var erpBag in erpBags)
            {
                var sku = SkusRepository.getSkuBySiesaId(erpBag.IdProducto).Result;
                if (sku.vtex_id != null)
                {
                    bags.Add(new Bag{
                            VtexId = (int)sku.vtex_id,
                            Weigth = erpBag.Peso
                        }
                    );
                }
            }
            return bags;
        }
    }
}