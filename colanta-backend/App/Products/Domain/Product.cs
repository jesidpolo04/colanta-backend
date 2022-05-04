namespace colanta_backend.App.Products.Domain
{
    using App.Categories.Domain;
    using App.Brands.Domain;
    using System.Collections.Generic;
    public class Product
    {
        public int? id { get; set; }
        public int? type { get; set; }
        public string siesa_id { get; set; }
        public string concat_siesa_id { get; set; }
        public int? vtex_id { get; set; }
        public string name { get; set; }
        public Category category { get; set; }
        public int category_id { get; set; }
        public Brand brand { get; set; }
        public int brand_id { get; set; }
        public List<Sku> skus { get; set; }
        public string? ref_id { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public string business { get; set; }

        public Product()
        {
            this.skus = new List<Sku>();
        }

        public void setBrand(Brand brand)
        {
            this.brand = brand;
        }

        public void setCategory(Category category)
        {
            this.category = category;
        }

        public void addSku(Sku sku)
        {
            this.skus.Add(sku);
        }

    }
}
