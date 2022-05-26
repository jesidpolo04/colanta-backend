namespace colanta_backend.App.Promotions.Infraestructure
{
    using App.Promotions.Domain;
    using App.Categories.Domain;
    using App.Categories.Infraestructure;
    using App.Brands.Domain;
    using App.Brands.Infraestructure;
    using App.Products.Domain;
    using App.Products.Infraestructure;
    using System.Collections.Generic;
    public class EFPromotion
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

        public decimal nominal_discount_value { get; set; }
        public decimal percentual_discount_value { get; set; }
        public decimal percentual_shipping_discount_value { get; set; }

        public string? max_number_of_affected_items_group_key { get; set; }
        public int max_number_of_affected_items { get; set; }

        public int minimum_quantity_buy_together { get; set; }
        public int quantity_to_affect_buy_together { get; set; }

        public string products_ids { get; set; }
        public string skus_ids { get; set; }
        public string brands_ids { get; set; }
        public string categories_ids { get; set; }

        public int gift_quantity_selectable { get; set; }
        public string gifts_ids { get; set; }

        public decimal percentual_discount_value_list_1 { get; set; }
        public decimal percentual_discount_value_list_2 { get; set; }

        public string list_sku_1_buy_together_ids { get; set; }
        public string list_sku_2_buy_together_ids { get; set; }


        public decimal total_value_floor { get; set; }
        public decimal total_value_celing { get; set; }
        public bool cumulative { get; set; }
        public bool multiple_use_per_client { get; set; }

        public Promotion getPromotionFromEfPromotion()
        {
            Promotion promotion = new Promotion();
            promotion.id = this.id;
            promotion.vtex_id = this.vtex_id;
            promotion.siesa_id = this.siesa_id;
            promotion.concat_siesa_id = this.concat_siesa_id;
            promotion.business = this.business;
            promotion.type = this.type;
            promotion.discount_type = this.discount_type;
            promotion.name = this.name;
            promotion.begin_date_utc = this.begin_date_utc;
            promotion.end_date_utc = this.end_date_utc;
            promotion.is_active = this.is_active;
            promotion.nominal_discount_value = this.nominal_discount_value;
            promotion.percentual_discount_value = this.percentual_discount_value;
            promotion.percentual_shipping_discount_value = this.percentual_shipping_discount_value;
            promotion.max_number_of_affected_items_group_key = this.max_number_of_affected_items_group_key;
            promotion.max_number_of_affected_items = this.max_number_of_affected_items;
            promotion.minimum_quantity_buy_together = this.minimum_quantity_buy_together;
            promotion.quantity_to_affect_buy_together = this.quantity_to_affect_buy_together;


            promotion.products_ids = this.products_ids;

            promotion.skus_ids = this.skus_ids;

            promotion.brands_ids = this.brands_ids;

            promotion.categories_ids = this.categories_ids;

            promotion.gift_quantity_selectable = this.gift_quantity_selectable;
  
            promotion.gifts_ids = this.gifts_ids;

            promotion.percentual_discount_value_list_1 = this.percentual_discount_value_list_1;
            promotion.percentual_discount_value_list_2 = this.percentual_discount_value_list_2;

            promotion.list_sku_1_buy_together_ids = this.list_sku_1_buy_together_ids;

            promotion.list_sku_2_buy_together_ids = this.list_sku_2_buy_together_ids;

            promotion.total_value_floor = this.total_value_floor;
            promotion.total_value_celing = this.total_value_celing;
            promotion.cumulative = this.cumulative;
            promotion.multiple_use_per_client = this.multiple_use_per_client;

            return promotion;
        }

        public void setEfPromotionFromPromotion(Promotion promotion)
        {
            this.vtex_id = promotion.vtex_id;
            this.siesa_id = promotion.siesa_id;
            this.concat_siesa_id = promotion.concat_siesa_id;
            this.business = promotion.business;
            this.type = promotion.type;
            this.discount_type = promotion.discount_type;
            this.name = promotion.name;
            this.begin_date_utc = promotion.begin_date_utc;
            this.end_date_utc = promotion.end_date_utc;
            this.is_active = promotion.is_active;
            this.nominal_discount_value = promotion.nominal_discount_value;
            this.percentual_discount_value = promotion.percentual_discount_value;
            this.percentual_shipping_discount_value = promotion.percentual_shipping_discount_value;
            this.max_number_of_affected_items_group_key = promotion.max_number_of_affected_items_group_key;
            this.max_number_of_affected_items = promotion.max_number_of_affected_items;
            this.minimum_quantity_buy_together = promotion.minimum_quantity_buy_together;
            this.quantity_to_affect_buy_together = promotion.quantity_to_affect_buy_together;
            this.products_ids = promotion.products_ids;
            this.skus_ids = promotion.skus_ids;
            this.brands_ids = promotion.brands_ids;
            this.categories_ids = promotion.categories_ids;
            this.gift_quantity_selectable = promotion.gift_quantity_selectable;
            this.gifts_ids = promotion.gifts_ids;
            this.percentual_discount_value_list_1 = promotion.percentual_discount_value_list_1;
            this.percentual_discount_value_list_2 = promotion.percentual_discount_value_list_2;
            this.list_sku_1_buy_together_ids = promotion.list_sku_1_buy_together_ids;
            this.list_sku_2_buy_together_ids = promotion.list_sku_2_buy_together_ids;
            this.total_value_floor = promotion.total_value_floor;
            this.total_value_celing = promotion.total_value_celing;
            this.cumulative = promotion.cumulative;
            this.multiple_use_per_client = promotion.multiple_use_per_client;
        }
    }
}
