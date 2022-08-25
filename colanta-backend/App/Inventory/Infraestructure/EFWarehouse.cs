namespace colanta_backend.App.Inventory.Infraestructure
{
    using App.Inventory.Domain;
    public class EFWarehouse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string siesa_id { get; set; }
        public string vtex_id { get; set; }
        public string business { get; set; }

        public Warehouse getWarehouseFromEfWarehouse()
        {
            Warehouse warehouse = new Warehouse();

            warehouse.id = id;
            warehouse.name = name;
            warehouse.email = email;
            warehouse.siesa_id = siesa_id;
            warehouse.vtex_id = vtex_id;
            warehouse.business = business;

            return warehouse;
        }

        public void setEfWarehouseFromWarehouse(Warehouse warehouse)
        {
            this.id = warehouse.id;
            this.name = warehouse.name;
            this.email = warehouse.email;
            this.siesa_id = warehouse.siesa_id;
            this.vtex_id = warehouse.vtex_id;
            this.business = warehouse.business;
        }
    }
}
