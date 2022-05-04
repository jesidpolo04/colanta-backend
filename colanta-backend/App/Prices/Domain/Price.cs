namespace colanta_backend.App.Prices.Domain
{
    using Products.Domain;
    public class Price
    {
        public int? id { get; set; }
        public decimal price { get; set; }
        public string sku_concat_siesa_id { get; set; }
        public int sku_id { get; set; }
        public Sku sku { get; set; }
        public string business { get; set; }

    }
}
