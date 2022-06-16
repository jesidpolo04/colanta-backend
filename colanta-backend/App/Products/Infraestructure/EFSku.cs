namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    public class EFSku
    {
        public virtual EFProduct product { get; set; }
        public int product_id { get; set; }
        public int? id { get; set; }
        public int? vtex_id { get; set; }
        public string siesa_id { get; set; }
        public string concat_siesa_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string? ref_id { get; set; }
        public double packaged_height { get; set; }
        public double packaged_length { get; set; }
        public double packaged_width { get; set; }
        public double packaged_weight_kg { get; set; }
        public bool is_active { get; set; }
        public string measurement_unit { get; set; }
        public decimal? unit_multiplier { get; set; }

        public Sku GetSkuFromEfSku()
        {
            Sku sku = new Sku();

            sku.id = this.id;
            sku.product_id = this.product_id;
            sku.vtex_id = this.vtex_id;
            sku.siesa_id = this.siesa_id;
            sku.concat_siesa_id = this.concat_siesa_id;
            sku.name = this.name;
            sku.description = this.description;
            sku.ref_id = this.ref_id;
            sku.packaged_weight_kg = this.packaged_weight_kg;
            sku.packaged_length = this.packaged_length;
            sku.packaged_width = this.packaged_width;
            sku.packaged_height = this.packaged_height;
            sku.is_active = this.is_active;
            sku.measurement_unit = this.measurement_unit;
            sku.unit_multiplier = this.unit_multiplier;

            if(this.product != null)
            {
                Product product = new Product();

                product.id = this.product.id;
                product.type = this.product.type;
                product.brand_id = this.product.brand_id;
                product.category_id = this.product.category_id;
                product.siesa_id = this.product.siesa_id;
                product.concat_siesa_id = this.product.concat_siesa_id;
                product.vtex_id = this.product.vtex_id;
                product.name = this.product.name;
                product.description = this.product.description;
                product.ref_id = this.product.ref_id;
                product.is_active = this.product.is_active;
                product.business = this.product.business;

                sku.setProduct(product);
            }

            return sku;
        }

        public void setEfSkuFromSku(Sku sku)
        {
            this.id = sku.id;
            this.product_id = sku.product_id;
            this.vtex_id = sku.vtex_id;
            this.siesa_id = sku.siesa_id;
            this.concat_siesa_id = sku.concat_siesa_id;
            this.name = sku.name;
            this.description = sku.description;
            this.ref_id = sku.ref_id;
            this.packaged_weight_kg = sku.packaged_weight_kg;
            this.packaged_length = sku.packaged_length;
            this.packaged_width = sku.packaged_width;
            this.packaged_height = sku.packaged_height;
            this.is_active = sku.is_active;
            this.measurement_unit = sku.measurement_unit;
            this.unit_multiplier = sku.unit_multiplier;
        }
    }
}
