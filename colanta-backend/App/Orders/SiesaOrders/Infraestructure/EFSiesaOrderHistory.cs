namespace colanta_backend.App.Orders.SiesaOrders.Infraestructure
{
    using System;
    using SiesaOrders.Domain;
    public class EFSiesaOrderHistory
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string vtex_id { get; set; }
        public string order_json { get; set; }
        public DateTime created_at { get; set; }

        public SiesaOrderHistory getSiesaOrderHistoryFromEfSiesaOrderHistory()
        {
            SiesaOrderHistory siesaOrderHistory = new SiesaOrderHistory();
            siesaOrderHistory.id = id;
            siesaOrderHistory.siesa_id = siesa_id;
            siesaOrderHistory.vtex_id = vtex_id;
            siesaOrderHistory.order_json = order_json;
            return siesaOrderHistory;
        }

        public void setEfSiesaOrderHistoryFromSiesaOrderHistory(SiesaOrderHistory siesaOrderHistory)
        {
            this.siesa_id = siesaOrderHistory.siesa_id;
            this.vtex_id = siesaOrderHistory.vtex_id;
            this.order_json = siesaOrderHistory.order_json;
        }
    }
}
