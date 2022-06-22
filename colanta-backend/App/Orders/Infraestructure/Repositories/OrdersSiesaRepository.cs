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
        private ProductsRepository productsLocalRepository;
        private PromotionsRepository promotionLocalRepository;
        private HttpClient httpClient;
        private IConfiguration configuration;
        
        public OrdersSiesaRepository(
            ProductsRepository productsLocalRepository,
            PromotionsRepository promotionLocalRepository,
            IConfiguration configuration
        )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.promotionLocalRepository = promotionLocalRepository;
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }
        public async Task<Order> saveOrder(Order order)
        {
            string endpoint = "/siesa_order_endpoint";
            VtexOrderToSiesaOrderMapper mapper = new VtexOrderToSiesaOrderMapper(this.productsLocalRepository, this.promotionLocalRepository);
            VtexOrderDto vtexOrderDto = JsonSerializer.Deserialize<VtexOrderDto>(order.order_json);
            SendOrderToSiesaDto siesaOrderDto = mapper.getSiesaOrder(vtexOrderDto);
            string jsonContent = JsonSerializer.Serialize(vtexOrderDto);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            await httpClient.PostAsync(this.configuration["SiesaUrl"] + endpoint, httpContent);
            System.Console.WriteLine(jsonContent);
            return order;
        }
    }
}
