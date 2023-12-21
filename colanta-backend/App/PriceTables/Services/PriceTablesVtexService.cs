using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using colanta_backend.App.Shared;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace colanta_backend.App.PriceTables
{
    public class PriceTablesVtexService
    {
        private IConfiguration _Configuration;
        private string _ApiKey;
        private string _ApiToken;
        private string _AccountName;
        private string _VtexEnvironment;
        private string _Host;
        private HttpClient _HttpClient;
        public PriceTablesVtexService(IConfiguration configuration)
        {
            _Configuration = configuration;
            _ApiKey = configuration["MercolantaVtexApiKey"];
            _ApiToken = configuration["MercolantaVtexToken"];
            _AccountName = configuration["MercolantaAccountName"];
            _VtexEnvironment = configuration["MercolantaEnvironment"];
            _Host = $"https://api.vtex.com/{_AccountName}";
            _HttpClient = new HttpClient();
            _HttpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", _ApiToken);
            _HttpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", _ApiKey);
        }

        public async Task<HttpResponse<bool?>> AddFixedPriceToPriceTable(FixedPrice fixedPrice)
        {
            string requestUri = $"{_Host}/pricing/prices/{fixedPrice.VtexSkuId}/fixed/{fixedPrice.PriceTableName}";
            var fixedPriceDto = FixedPriceDto.GetDtoFromFixedPrice(fixedPrice);
            FixedPriceDto[] fixedPriceDtoArray = { fixedPriceDto };
            string jsonContent = JsonConvert.SerializeObject(fixedPriceDtoArray);
            HttpContent httpContent = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            var httpResponse = await _HttpClient.PostAsync(requestUri, httpContent);
            return new HttpResponse<bool?>
            {
                Data = null,
                IsSuccessStatusCode = httpResponse.IsSuccessStatusCode,
                Status = httpResponse.StatusCode
            };
        }
    }
}