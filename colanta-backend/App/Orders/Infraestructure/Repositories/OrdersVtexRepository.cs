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
    using App.Shared.Infraestructure;
    using colanta_backend.App.Orders.Domain;
    using colanta_backend.App.Orders.SiesaOrders.Domain;

    public class OrdersVtexRepository : VtexRepository, Domain.OrdersVtexRepository
    {
        public OrdersVtexRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task approvePayment(string paymentId, string orderVtexId)
        {
            string endpoint = $"/api/oms/pvt/orders/{orderVtexId}/payments/{paymentId}/payment-notification";
            string url = $"https://{accountName}.{vtexEnvironment}{endpoint}";
            HttpResponseMessage vtexResponse = await httpClient.PostAsync(url, null);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
        }

        public async Task cancelOrder(string orderVtexId)
        {
            string endpoint = $"/api/oms/pvt/orders/{orderVtexId}/cancel";
            string url = $"https://{accountName}.{vtexEnvironment}{endpoint}";
            var requestBody = new
            {
                reason = "El pedido no se completó"
            };
            HttpContent content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
                );
            HttpResponseMessage vtexResponse = await httpClient.PostAsync(url, content);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
        }

        public async Task<VtexOrder> getOrderByVtexId(string vtexOrderId)
        {
            string endpoint = "/api/oms/pvt/orders/";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint + vtexOrderId;
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (vtexResponse.StatusCode != System.Net.HttpStatusCode.OK && vtexResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
            string vtexResponseBody = await vtexResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VtexOrder>(vtexResponseBody);
        }

        public async Task startHandlingOrder(string orderVtexId)
        {
            string endpoint = "/api/oms/pvt/orders/" + orderVtexId + "/start-handling";
            string url = "https://" + this.accountName + "." + this.vtexEnvironment + endpoint;
            HttpContent httpContent = new StringContent("", System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = await this.httpClient.PostAsync(url, httpContent);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status {vtexResponse.StatusCode}");
            }
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
