namespace colanta_backend.App.Shared
{
    using System;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class HttpSSLHandler : HttpClientHandler
    {
        private readonly ILogger<HttpSSLHandler> _logger;

        public HttpSSLHandler(ILogger<HttpSSLHandler> logger)
        {
            _logger = logger;
            ServerCertificateCustomValidationCallback = ValidateServerCertificate;
        }

        private bool ValidateServerCertificate(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
        {
            if (sslErrors != SslPolicyErrors.None)
            {
                _logger.LogError($"SSL Certificate error: {sslErrors}");
                foreach (var chainStatus in chain.ChainStatus)
                {
                    _logger.LogError($"Chain status: {chainStatus.Status} - {chainStatus.StatusInformation}");
                }
            }
            return sslErrors == SslPolicyErrors.None;
        }
    }
}
