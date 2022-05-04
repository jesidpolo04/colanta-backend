namespace colanta_backend.App.Products.Infraestructure
{
    using System.Collections.Generic;
    using Products.Domain;
    using Categories.Infraestructure;
    using Brands.Infraestructure;
    public class EFProduct
    {
        public int? id { get; set; }
        public int? type { get; set; }
        public string siesa_id { get; set; }
        public string concat_siesa_id { get; set; }
        public int? vtex_id { get; set; }
        public string name { get; set; }
        public string? ref_id { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public string business { get; set; }
        public virtual EFCategory category { get; set; }
        public int category_id { get; set; }
        public virtual EFBrand brand { get; set; }
        public int brand_id { get; set; }
        public virtual List<EFSku> skus { get; set; }

        public Product getProductFromEfProduct()
        {
            Product product = new Product();

            product.id = this.id;
            product.type = this.type;
            product.brand_id = this.brand_id;
            product.category_id = this.category_id;
            product.siesa_id = this.siesa_id;
            product.concat_siesa_id = this.concat_siesa_id;
            product.vtex_id = this.vtex_id;
            product.name = this.name;
            product.description = this.description;
            product.ref_id = this.ref_id;
            product.is_active = this.is_active;
            product.business = this.business;

            product.category = this.category.getCategoryFromEFCategory();
            product.brand = this.brand.getBrandFromEFBrand();

            return product;
        }

        public void setEfProductFromProduct(Product product)
        {
            this.id = product.id;
            this.type = product.type;
            this.brand_id = product.brand_id;
            this.category_id = product.category_id;
            this.siesa_id = product.siesa_id;
            this.concat_siesa_id = product.concat_siesa_id;
            this.vtex_id = product.vtex_id;
            this.name = product.name;
            this.description = product.description;
            this.ref_id = product.ref_id;
            this.is_active = product.is_active;
            this.business = product.business;
        }
    }
}
