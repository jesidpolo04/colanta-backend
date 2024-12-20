using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using colanta_backend.App.Shared.Domain;
using colanta_backend.App.Shared.Infraestructure;
using colanta_backend.App.Taxes.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace colanta_backend.App.Taxes.Services
{
    public class TaxService
    {
        private SiesaAuth _SiesaAuth;
        private HttpClient _HttpClient;
        private IConfiguration _Configuration;
        public TaxService(IConfiguration Configuration, SiesaAuth SiesaAuth)
        {
            _Configuration = Configuration;
            _SiesaAuth = SiesaAuth;
            _HttpClient = new HttpClient();
        }

        public async Task<ProductSiesaTaxes[]> GetSiesaTaxes()
        {
            SetHeaders();
            string endpoint = "/api/ColantaWS/Impuestos";
            HttpResponseMessage response = await _HttpClient.GetAsync($"{_Configuration["SiesaUrl"]}{endpoint}");
            if (!response.IsSuccessStatusCode)
            {
                throw new SiesaException(response, $"Siesa respondió con status: {response.StatusCode}");
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            var parsedBody = JsonConvert.DeserializeObject<SiesaTaxesResponse>(responseBody);
            return parsedBody.Impuestos;
        }

        public ProductSiesaTaxes? FindProductTaxes(ProductSiesaTaxes[] taxesList, string refId){
            var  productTaxesList = taxesList.Where( taxes => taxes.IdProducto == refId).ToList();
            if(productTaxesList.Count > 0) return productTaxesList.First();
            else return null;
        } 

        private void SetHeaders()
        {
            _HttpClient.DefaultRequestHeaders.Remove("Authorization");
            _HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_SiesaAuth.getToken().Result}");
        }
    }
}