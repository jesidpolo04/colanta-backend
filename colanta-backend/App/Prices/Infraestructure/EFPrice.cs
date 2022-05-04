namespace colanta_backend.App.Prices.Infraestructure
{
    using Prices.Domain;
    using Products.Domain;
    using Products.Infraestructure;
    public class EFPrice
    {
        public int? id { get; set; }
        public decimal price { get; set; }
        public string sku_concat_siesa_id { get; set; }
        public int sku_id { get; set; }
        public EFSku sku { get; set; }
        public string business { get; set; }

        public void setEfPriceFromPrice(Price price)
        {
            this.id = price.id;
            this.price = price.price;
            this.sku_concat_siesa_id = price.sku_concat_siesa_id;
            this.sku_id = price.sku_id;
            this.business = price.business;
        }

        public Price getPriceFromEfPrice()
        {
            Price price = new Price();
            price.id = this.id;
            price.price = this.price;
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
            price.sku = sku;

            return price;
        }
    }
}
