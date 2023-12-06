using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using colanta_backend.App.Shared.Domain;
using colanta_backend.App.Shared.Infraestructure;
using Microsoft.Extensions.Configuration;

namespace colanta_backend.App.Taxes.Services
{
    public class TaxService
    {
        private SiesaAuth _SiesaAuth;
        private HttpClient _HttpClient;
        private IConfiguration _Configuration;

        public TaxService(IConfiguration Configuration)
        {
            _Configuration = Configuration;
            _SiesaAuth = new SiesaAuth(Configuration);
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
            var parsedBody = JsonSerializer.Deserialize<SiesaTaxesResponse>(responseBody);
            return parsedBody.Impuestos;
        }

        private void SetHeaders()
        {
            _HttpClient.DefaultRequestHeaders.Remove("Authorization");
            _HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_SiesaAuth.getToken().Result}");
        }
    }
}