using colanta_backend.App.Orders.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Products.Domain;
    using Promotions.Domain;
    using System.Text.Json;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    public class OrdersSiesaRepository : Domain.OrdersSiesaRepository
    {
        private SkusRepository skusLocalRepository;
        private PromotionsRepository promotionLocalRepository;
        private HttpClient httpClient;
        private IConfiguration configuration;
        
        public OrdersSiesaRepository(
            SkusRepository skusLocalRepository,
            PromotionsRepository promotionLocalRepository,
            IConfiguration configuration
        )
        {
            this.skusLocalRepository = skusLocalRepository;
            this.promotionLocalRepository = promotionLocalRepository;
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }
        public async Task<Order> saveOrder(Order order)
        {
            string endpoint = "/siesa_order_endpoint";
            VtexOrderToSiesaOrderMapper mapper = new VtexOrderToSiesaOrderMapper(this.skusLocalRepository, this.promotionLocalRepository);
            VtexOrderDto vtexOrderDto = JsonSerializer.Deserialize<VtexOrderDto>(order.order_json);
            SendOrderToSiesaDto siesaOrderDto = await mapper.getSiesaOrder(vtexOrderDto);
            string jsonContent = JsonSerializer.Serialize(siesaOrderDto);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            await httpClient.PostAsync(this.configuration["SiesaUrl"] + endpoint, httpContent);
            System.Console.WriteLine(jsonContent);
            return order;
        }
    }
}
