namespace colanta_backend.App.Products.Domain
{
    public class Sku
    {
        public Product product { get; set; }
        public int product_id { get; set; }
        public int? id { get; set; }
        public int? vtex_id { get; set; }
        public string siesa_id { get; set; }
        public string ean { get; set; }
        public string concat_siesa_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string? ref_id { get; set; }
        public double packaged_height { get; set; }
        public double packaged_length { get; set; }
        public double packaged_width { get; set; }
        public double packaged_weight_kg { get; set; }
        public double? height { get; set; }
        public double? length { get; set; }
        public double? width { get; set; }
        public double? weight_kg { get; set; }
        public double? cubic_weight { get; set; }
        public bool? is_kit  { get; set; }
        public bool is_active { get; set; }
        public string? creation_date { get; set; }
        public double? reward_value { get; set; }
        public string? estimated_date_arrival { get; set; }
        public string? manufacturer_code { get; set; }
        public int? commercial_condition_id { get; set; }
        public string measurement_unit { get; set; }
        public decimal? unit_multiplier = 1;
        public string? modal_type { get; set; }
        public bool? kit_itens_sell_apart { get; set; }
        public string? videos { get; set; }

        public void setProduct(Product product)
        {
            this.product = product;
            
        }

    }
}
