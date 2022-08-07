﻿using colanta_backend.App.Orders.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Products.Domain;
    using Promotions.Domain;
    using System.Text.Json;
    using System.Net.Http;
    using Shared.Domain;
    using Shared.Infraestructure;
    using Microsoft.Extensions.Configuration;
    using colanta_backend.App.Orders.SiesaOrders.Domain;

    public class OrdersSiesaRepository : Domain.OrdersSiesaRepository
    {
        private SkusRepository skusLocalRepository;
        private PromotionsRepository promotionLocalRepository;
        private HttpClient httpClient;
        private SiesaAuth siesaAuth;
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
            this.siesaAuth = new SiesaAuth(configuration);
        }

        public async Task<SiesaOrder> getOrderBySiesaId(string siesaId)
        {
            string endpoint = "/ordenes/" + siesaId;
            HttpResponseMessage siesaResponse = await httpClient.GetAsync("http://localhost:3333" + endpoint);
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            UpdatedSiesaOrderResponseDto siesaOrderDto = JsonSerializer.Deserialize<UpdatedSiesaOrderResponseDto>(siesaResponseBody);
            SiesaOrder siesaOrder = siesaOrderDto.pedido.getSiesaOrderFromDto();
            siesaOrder.finalizado = siesaOrderDto.finalizado;
            return siesaOrder;
        }

        public async Task<SiesaOrder> saveOrder(Order order)
        {
            this.setHeaders().Wait();
            string endpoint = "/api/ColantaWS/EnviarPedido";
            VtexOrderToSiesaOrderMapper mapper = new VtexOrderToSiesaOrderMapper(this.skusLocalRepository, this.promotionLocalRepository);
            VtexOrderDto vtexOrderDto = JsonSerializer.Deserialize<VtexOrderDto>(order.order_json);
            SiesaOrderDto siesaOrderDto = await mapper.getSiesaOrderDto(vtexOrderDto);
            string jsonContent = JsonSerializer.Serialize(siesaOrderDto);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage siesaResponse = await httpClient.PostAsync(this.configuration["SiesaUrl"] + endpoint, httpContent);
            string siesaResponseBody = await siesaResponse.Content.ReadAsStringAsync();
            if (!siesaResponse.IsSuccessStatusCode)
            {
                throw new SiesaException(siesaResponse, $"Siesa respondió con status: {siesaResponse.StatusCode}");
            }
            SiesaOrderIdResponseDto siesaOrderIdResponseDto = JsonSerializer.Deserialize<SiesaOrderIdResponseDto>(siesaResponseBody);
            SiesaOrder siesaOrder = siesaOrderDto.getSiesaOrderFromDto();
            siesaOrder.siesa_id = siesaOrderIdResponseDto.id;
            siesaOrder.finalizado = false;
            return siesaOrder;
        }

        private async Task setHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("Authorization");
            this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await this.siesaAuth.getToken());
        }
    }
}
