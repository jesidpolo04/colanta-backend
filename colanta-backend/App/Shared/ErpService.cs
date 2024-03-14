using System;
using System.Net.Http;
using colanta_backend.App.Shared.Infraestructure;
using Microsoft.Extensions.Configuration;

namespace colanta_backend.App.Shared
{
    public class ErpService
    {
        protected SiesaAuth SiesaAuth { get; set; }
        protected HttpClient HttpClient { get; set; }
        protected IConfiguration Configuration { get; set; }
        protected String BaseUrl { get; set; }
        public ErpService(IConfiguration configuration)
        {
            Configuration = configuration;
            SiesaAuth = new SiesaAuth(configuration);
            HttpClient = new HttpClient();
            BaseUrl = configuration["SiesaUrl"];
        }

        protected void SetHeaders()
        {
            HttpClient.DefaultRequestHeaders.Remove("Authorization");
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SiesaAuth.getToken().Result}");
        }
    }
}