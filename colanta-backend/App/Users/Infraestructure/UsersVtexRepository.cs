namespace colanta_backend.App.Users.Infraestructure
{
    using colanta_backend.App.Users.Domain;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Users.Domain;
    using Shared.Domain;
    using System.Text.Json;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class UsersVtexRepository : Domain.UsersVtexRepository
    {
        private IConfiguration configuration;
        private HttpClient httpClient;
        private string apiKey;
        private string apiToken;
        private string accountName;
        private string vtexEnvironment;

        public UsersVtexRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnvironment = configuration["MercolantaEnvironment"];

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.setCredentialHeaders();
        }

        private void setCredentialHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppToken");
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppKey");

            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);

        }

        public async Task<VtexUser> getByDocumentAndEmail(string document, string email)
        {
            string queryFields = "_fields=id,document,documentType,email,firstName,lastName,phone,homePhone,birthDate,address";
            string queryConditions = $"_where=(document={document})AND(email={email})";
            string endpoint = $"/api/dataentities/CL/search?{queryFields}&{queryConditions}";
            string url = $"https://{accountName}.{vtexEnvironment}{endpoint}";
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status: {vtexResponse.StatusCode}");
            }
            string responseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexUser[] vtexUsers = JsonSerializer.Deserialize<VtexUser[]>(responseBody);
            if (vtexUsers.Length == 0) return null;
            else return vtexUsers.First();
        }

        public async Task<VtexUser> getByEmail(string email)
        {
            string queryFields = "_fields=id,document,documentType,email,firstName,lastName,phone,homePhone,birthDate,address";
            string queryConditions = $"_where=(email={email})";
            string endpoint = $"/api/dataentities/CL/search?{queryFields}&{queryConditions}";
            string url = $"https://{accountName}.{vtexEnvironment}{endpoint}";
            HttpResponseMessage vtexResponse = await httpClient.GetAsync(url);
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status: {vtexResponse.StatusCode}");
            }
            string responseBody = await vtexResponse.Content.ReadAsStringAsync();
            VtexUser[] vtexUsers = JsonSerializer.Deserialize<VtexUser[]>(responseBody);
            if (vtexUsers.Length == 0) return null;
            else return vtexUsers.First();
        }

        public Task setCustomerClass(string vtexId, string customerClass)
        {
            string endpoint = $"/api/dataentities/CL/documents/{vtexId}";
            string url = $"https://{accountName}.{vtexEnvironment}{endpoint}";
            object requestBody = new
            {
                customerClass = customerClass
            };
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage vtexResponse = httpClient.PostAsync(url, httpContent).Result;
            if (!vtexResponse.IsSuccessStatusCode)
            {
                throw new VtexException(vtexResponse, $"Vtex respondió con status: {vtexResponse.StatusCode}");
            }
            return Task.CompletedTask;
        }
    }
}
