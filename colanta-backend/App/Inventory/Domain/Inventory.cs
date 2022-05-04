namespace colanta_backend.App.Inventory.Domain
{
    using App.Products.Domain;
    public class Inventory
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public int sku_id { get; set; }
        public string sku_concat_siesa_id { get; set; }
        public Sku sku { get; set; }
        public int warehouse_id { get; set; }
        public string warehouse_siesa_id { get; set; }
        public Warehouse warehouse { get; set; }
    }
}
