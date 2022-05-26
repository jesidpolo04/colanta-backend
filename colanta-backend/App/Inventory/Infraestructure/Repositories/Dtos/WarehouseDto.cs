namespace colanta_backend.App.Inventory.Infraestructure
{
    using App.Inventory.Domain;
    public class WarehouseDto
    {
        public string id { get; set; }
        public string nombre { get; set; }
        
        public Warehouse getWarehouseFromDto()
        {
            Warehouse warehouse = new Warehouse();
            warehouse.name = this.nombre;
            warehouse.vtex_id = this.id;
            warehouse.siesa_id = this.id;
            warehouse.business = "mercolanta";
            return warehouse;
        }
    }
}
