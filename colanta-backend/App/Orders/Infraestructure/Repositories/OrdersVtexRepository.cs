namespace colanta_backend.App.Orders.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using App.Shared.Domain;
    using colanta_backend.App.Orders.Domain;
    using colanta_backend.App.Orders.SiesaOrders.Domain;

    public class OrdersVtexRepository : Domain.OrdersVtexRepository
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;
        public OrdersVtexRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnvironment = configuration["MercolantaEnvironment"];

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);
        }

        public async Task<string> getOrderByVtexId(string vtexOrderId)
        {
            string endpoint = "/api/oms/pvt/orders/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexOrderId;
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            return vtexResponseBody;
        }

        public async Task<PaymentMethod> getOrderPaymentMethod(string orderVtexId)
        {
            string endpoint = "/api/oms/pvt/orders/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + orderVtexId;
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexOrderDto vtexOrderDto = JsonSerializer.Deserialize<VtexOrderDto>(vtexResponseBody);
            PaymentMethod paymentMethod = new PaymentMethod();
            paymentMethod.vtex_id = vtexOrderDto.paymentData.transactions[0].payments[0].paymentSystem;
            paymentMethod.name = vtexOrderDto.paymentData.transactions[0].payments[0].paymentSystemName;
            return paymentMethod;
        }

        public async Task<OrderStatus> getOrderStatus(string orderVtexId)
        {
            string endpoint = "/api/oms/pvt/orders/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + orderVtexId;
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexOrderDto vtexOrderDto = JsonSerializer.Deserialize<VtexOrderDto>(vtexResponseBody);
            OrderStatus orderStatus = new OrderStatus();
            orderStatus.status = vtexOrderDto.status;
            return orderStatus;
        }

        public async Task<bool> startHandlingOrder(string orderVtexId)
        {
            string endpoint = "/api/oms/pvt/orders/" + orderVtexId + "/start-handling";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            HttpContent httpContent = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            return true;
        }

        public async Task<string> updateVtexOrder(SiesaOrder oldSiesaOrder, SiesaOrder newSiesaOrder)
        {
            string endpoint = "/api/oms/pvt/orders/" + oldSiesaOrder.referencia_vtex + "/changes";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;

            string reason = "El pedido fue actualizado y facturado";
            decimal discountValue = 0;
            decimal incrementValue = 0;
            decimal oldSiesaOrderValue = oldSiesaOrder.total_pedido;
            decimal newSiesaOrderValue = newSiesaOrder.total_pedido;
            List<AddedItem> addedItems = oldSiesaOrder.getAddedItems(newSiesaOrder.detalles);
            List<RemovedItem> removedItems = oldSiesaOrder.getRemovedItems(newSiesaOrder.detalles);

            if (oldSiesaOrderValue > newSiesaOrderValue)
            {
                decimal diff = oldSiesaOrderValue - newSiesaOrderValue;
                discountValue = discountValue + diff;
            }
            if(oldSiesaOrderValue < newSiesaOrderValue)
            {
                decimal diff = newSiesaOrderValue - oldSiesaOrderValue;
                incrementValue = incrementValue + diff;
            }

            UpdateVtexOrderDto request = new UpdateVtexOrderDto();

            request.generateRequestId(newSiesaOrder.referencia_vtex);
            request.incrementValue = Decimal.ToInt32(incrementValue);
            request.discountValue = Decimal.ToInt32(discountValue);
            request.reason = reason;
            foreach(AddedItem itemAdded in addedItems)
            {
                request.addItem(itemAdded.vtexId, itemAdded.price, itemAdded.quantity);
            }
            foreach(RemovedItem removedItem in removedItems)
            {
                request.removeItem(removedItem.vtexId, removedItem.price, removedItem.quantity);
            }

            string jsonRequest = JsonSerializer.Serialize(request);
            Console.WriteLine( "La Request:" + jsonRequest);
            HttpContent httpContent = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await httpClient.PostAsync(url, httpContent);
            Console.WriteLine("La Response:" + await vtexResponse.Content.ReadAsStringAsync());
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            
            return await vtexResponse.Content.ReadAsStringAsync();
        }
    }
}
