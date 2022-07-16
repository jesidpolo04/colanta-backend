namespace colanta_backend.App.Shared.Domain
{
    using System;
    using System.Net.Http;
    public class SiesaException : Exception
    {
        public int status;
        public string reponseBody;
        public string requestUrl;
        public string requestBody;
        public SiesaException(HttpResponseMessage httpResponse, string message)
            : base(message)
        {
            this.status = (int) httpResponse.StatusCode;
            this.reponseBody = httpResponse.Content.ReadAsStringAsync().Result;
            this.requestUrl = httpResponse.RequestMessage.RequestUri.ToString();
            this.reponseBody = httpResponse.RequestMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
