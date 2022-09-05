namespace colanta_backend.App.Orders.Domain
{
    using System;
    using Shared.Domain;
    using System.Net.Http;
    public class SiesaOrderRejectException : SiesaException
    {
        private Order order;
        public SiesaOrderRejectException(HttpResponseMessage response, Order order, string message) 
            : base(response, message)
        {
            this.order = order;
        }
    }
}
