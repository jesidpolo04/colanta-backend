namespace colanta_backend.App.Shared.Domain
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    public class SiesaException : Exception
    {
        public int status;
        public string responseBody;
        public string requestUrl;
        public string requestBody;

        public SiesaException(HttpResponseMessage httpResponse, string message)
            : base(message)
        {
            this.status = (int) httpResponse.StatusCode;
            this.responseBody = httpResponse.Content.ReadAsStringAsync().Result;
            this.requestUrl = httpResponse.RequestMessage.RequestUri.ToString();
            this.requestBody = httpResponse.RequestMessage.Content.ReadAsStringAsync().Result;
        }

        public override string ToString()
        {
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize(new
            {
                status = this.status,
                requestUrl = this.requestUrl,
                reponseBody = this.responseBody,
                requestBody = this.requestBody,
                stackTrace = this.StackTrace
            }, jsonOptions);
        }

    }
}
