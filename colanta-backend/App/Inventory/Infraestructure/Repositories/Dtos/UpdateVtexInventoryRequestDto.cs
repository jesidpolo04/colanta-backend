namespace colanta_backend.App.Inventory.Infraestructure
{
    public class UpdateVtexInventoryRequestDto
    {
        public bool unlimitedQuantity { get; set; }
        public int quantity { get; set; }
        public string? dateUtcOnBalanceSystem = null;
    }
}
