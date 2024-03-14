namespace colanta_backend.App.Inventory.Infraestructure
{
    using App.Inventory.Domain;
    public class SiesaInventoriesDto
    {
        public SiesaInventoryDto[] inventario_almacen { get; set; }
    }

    public class SiesaInventoryDto
    {
        public string id_producto { get; set; }
        public string? id_variacion { get; set; }
        public int cantidad { get; set; }
        public string negocio { get; set; }
        public bool infinito { get; set; }
        public int stockseguridad { get; set; }

        public Inventory getInventoryFromDto()
        {
            Inventory inventory = new Inventory();

            inventory.quantity = cantidad;
            inventory.business = negocio;
            inventory.infinite = infinito;
            inventory.security_stock = stockseguridad;
            
            if (id_variacion != null)
            {
                inventory.sku_concat_siesa_id = this.negocio + "_" + this.id_producto + "_" + id_variacion;
            }
            else
            {
                inventory.sku_concat_siesa_id = this.negocio + "_" + this.id_producto + "_" + id_producto;
            }
            return inventory;
        }


    }
}
