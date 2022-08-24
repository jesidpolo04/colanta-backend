namespace colanta_backend.App.Shared.Infraestructure
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.Extensions.Configuration;
    public abstract class VtexRepository 
    {
        protected IConfiguration configuration;
        protected HttpClient httpClient;
        protected string apiKey;
        protected string apiToken;
        protected string accountName;
        protected string vtexEnvironment;

        public VtexRepository(IConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.configuration = configuration;
            this.apiKey = configuration["MercolantaVtexApiKey"];
            this.apiToken = configuration["MercolantaVtexToken"];
            this.accountName = configuration["MercolantaAccountName"];
            this.vtexEnvironment = configuration["MercolantaEnvironment"];

            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.setCredentialHeaders();
        }

        protected void setCredentialHeaders()
        {
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppToken");
            this.httpClient.DefaultRequestHeaders.Remove("X-VTEX-API-AppKey");

            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppToken", this.apiToken);
            this.httpClient.DefaultRequestHeaders.Add("X-VTEX-API-AppKey", this.apiKey);
        }
    }
}
