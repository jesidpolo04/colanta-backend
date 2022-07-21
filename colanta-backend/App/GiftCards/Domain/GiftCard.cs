namespace colanta_backend.App.GiftCards.Domain
{
    using System;
    public class GiftCard
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string token { get; set; }
        public string business { get; set; }
        public decimal balance { get; set; }
        public string owner { get; set; }
        public string emision_date { get; set; }
        public string expire_date { get; set; }
    }
}
