namespace colanta_backend.App.Prices.Infraestructure
{
    using Prices.Domain;
    using Products.Domain;
    using Products.Infraestructure;
    public class EFPrice
    {
        public int? id { get; set; }
        public decimal price { get; set; }
        public decimal base_price { get; set; }
        public string sku_concat_siesa_id { get; set; }
        public int sku_id { get; set; }
        public EFSku sku { get; set; }
        public string business { get; set; }

        public void setEfPriceFromPrice(Price price)
        {
            this.id = price.id;
            this.price = price.price;
            this.base_price = price.base_price;
            this.sku_concat_siesa_id = price.sku_concat_siesa_id;
            this.sku_id = price.sku_id;
            this.business = price.business;
        }

        public Price getPriceFromEfPrice()
        {
            Price price = new Price();
            price.id = this.id;
            price.price = this.price;
            price.base_price = this.base_price;
            price.sku_concat_siesa_id = this.sku_concat_siesa_id;
            price.sku_id = this.sku_id;
            price.business = this.business;

            Sku sku = new Sku();

            sku.id = this.sku.id;
            sku.ref_id = this.sku.ref_id;
            sku.siesa_id = this.sku.siesa_id;
            sku.concat_siesa_id = this.sku.concat_siesa_id;
            sku.vtex_id = this.sku.vtex_id;
            sku.name = this.sku.name;
            sku.description = this.sku.description;
            sku.measurement_unit = this.sku.measurement_unit;
            sku.unit_multiplier = this.sku.unit_multiplier;
            sku.packaged_weight_kg = this.sku.packaged_weight_kg;
            sku.packaged_length = this.sku.packaged_length;
            sku.packaged_height = this.sku.packaged_height;
            sku.packaged_width = this.sku.packaged_width;

            Product product = new Product();
            product.id = this.sku.product.id;
            product.name = this.sku.product.name;
            product.ean = this.sku.product.ean;
            product.description = this.sku.product.description;
            product.ref_id = this.sku.product.ref_id;
            product.business = this.sku.product.business;
            product.type = this.sku.product.type;
            product.brand_id = this.sku.product.brand_id;
            product.category_id = this.sku.product.category_id;
            product.concat_siesa_id = this.sku.product.concat_siesa_id;
            product.vtex_id = this.sku.product.vtex_id;
            product.siesa_id = this.sku.product.siesa_id;
            product.is_active = this.sku.product.is_active;

            sku.product = product;
            price.sku = sku;

            return price;
        }
    }
}
