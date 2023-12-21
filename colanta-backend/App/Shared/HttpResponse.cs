using System.Net;

namespace colanta_backend.App.Shared{
    public class HttpResponse<T>{
        public bool IsSuccessStatusCode { get; set; }
        public HttpStatusCode Status { get; set; }
        public T? Data { get; set; }
    }
}