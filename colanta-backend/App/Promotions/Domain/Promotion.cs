namespace colanta_backend.App.Promotions.Domain
{
    using Products.Domain;
    using Categories.Domain;
    using Brands.Domain;
    public class Promotion
    {

        public int id { get; set; }
        public string? vtex_id { get; set; }
        public string? siesa_id { get; set; }
        public string? concat_siesa_id { get; set; }
        public string? business { get; set; }
        public string? type { get; set; }
        public string? discount_type { get; set; }
        public string? name { get; set; }

        public string begin_date_utc { get; set; }
        public string end_date_utc { get; set; }
        public bool is_active { get; set; }

        public decimal maximum_unit_price_discount { get; set; }
        public decimal nominal_discount_value { get; set; }
        public decimal percentual_discount_value { get; set; }
        public decimal percentual_shipping_discount_value { get; set; }

        public string? max_number_of_affected_items_group_key { get; set; }
        public int max_number_of_affected_items { get; set; }

        public int minimum_quantity_buy_together { get; set; }
        public int quantity_to_affect_buy_together { get; set; }

        public Product[] products { get; set; }
        public string products_ids { get; set; }
        public Sku[] skus { get; set; }
        public string skus_ids { get; set; }
        public Brand[] brands { get; set; }
        public string brands_ids { get; set; }
        public Category[] categories { get; set; }
        public string categories_ids { get; set; }
        public string cluster_expressions { get; set; }

        public int gift_quantity_selectable { get; set; }
        public Sku[] gifts { get; set; }
        public string gifts_ids { get; set; }

        public decimal percentual_discount_value_list_1 { get; set; }
        public decimal percentual_discount_value_list_2 { get; set; }

        public Sku[] list_sku_1_buy_together { get; set; }
        public string list_sku_1_buy_together_ids { get; set; }
        public Sku[] list_sku_2_buy_together { get; set; }
        public string list_sku_2_buy_together_ids { get; set; }

        public decimal total_value_floor { get; set; }
        public decimal total_value_celing { get; set; }
        public bool cumulative { get; set; }
        public bool multiple_use_per_client { get; set; }

    }
}
