namespace colanta_backend.App.Inventory.Infraestructure
{
    using App.Products.Infraestructure;
    using App.Inventory.Domain;
    using App.Products.Domain;
    public class EFInventory
    {
        public int id { get; set; }
        public int quantity { get; set; }
        public int sku_id { get; set; }
        public string sku_concat_siesa_id { get; set; }
        public EFSku sku { get; set; }
        public int warehouse_id { get; set; }
        public string warehouse_siesa_id { get; set; }
        public EFWarehouse warehouse { get; set; }
        public string business { get; set; }
        public bool infinite { get; set; }
        public int security_stock { get; set; }

        public Inventory getInventoryFromEfInventory()
        {
            Inventory inventory = new Inventory();
            inventory.id = this.id;
            inventory.quantity = this.quantity;
            inventory.sku_id = this.sku_id;
            inventory.sku_concat_siesa_id = this.sku_concat_siesa_id;
            inventory.warehouse_id = this.warehouse_id;
            inventory.warehouse_siesa_id = this.warehouse_siesa_id;
            inventory.business = this.business;
            inventory.infinite = this.infinite;
            inventory.security_stock = this.security_stock;

            Warehouse warehouse = new Warehouse();
            warehouse.id = this.warehouse.id;
            warehouse.name = this.warehouse.name;
            warehouse.siesa_id = this.warehouse.siesa_id;
            warehouse.vtex_id = this.warehouse.vtex_id;
            warehouse.business = this.warehouse.business;
            inventory.warehouse = warehouse;

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
            inventory.sku = sku;

            return inventory;
        }

        public void setEfInventoryFromInventory(Inventory inventory) 
        {
            this.id = inventory.id;
            this.quantity = inventory.quantity;
            this.sku_concat_siesa_id = inventory.sku_concat_siesa_id;
            this.warehouse_siesa_id = inventory.warehouse_siesa_id;
            this.business = inventory.business;
            this.infinite = inventory.infinite;
            this.security_stock = inventory.security_stock;
        }
    }
}
