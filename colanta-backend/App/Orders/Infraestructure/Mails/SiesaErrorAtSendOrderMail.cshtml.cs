using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Orders.Infraestructure
{
    using Shared.Domain;
    public class SiesaErrorAtSendOrderMailModel : PageModel
    {
        public SiesaException exception;
        public string vtexOrderId;
        public string requestBody;
        public string responseBody;
        public string status;

        public SiesaErrorAtSendOrderMailModel(SiesaException exception, string vtexOrderId)
        {
            this.exception = exception;
            this.vtexOrderId = vtexOrderId;
            status = exception.status.ToString();
            requestBody = exception.requestBody;
            responseBody = exception.responseBody;
        }

        public void OnGet()
        {
        }
    }
}
