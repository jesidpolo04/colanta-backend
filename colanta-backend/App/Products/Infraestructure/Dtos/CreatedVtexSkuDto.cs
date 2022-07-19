namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    public class CreatedVtexSkuDto
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
        public string? Name { get; set; }
        public string? RefId { get; set; }
        public double PackagedHeight { get; set; }
        public double PackagedLength { get; set; }
        public double PackagedWidth { get; set; }
        public double PackagedWeightKg { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? WeightKg { get; set; }
        public double? CubicWeight { get; set; }
        public bool IsKit { get; set; }
        public string? CreationDate { get; set; }
        public decimal? RewardValue { get; set; }
        public string? EstimatedDateArrival { get; set; }
        public string? ManufacturerCode { get; set; }
        public int? CommercialConditionId { get; set; }
        public string? MeasurementUnit { get; set; }
        public decimal? UnitMultiplier { get; set; }
        public string? ModalType { get; set; }
        public bool KitItensSellApart { get; set; }

        public Sku getSkuFromDto()
        {
            Sku sku = new Sku();

            sku.vtex_id = this.Id;
            sku.name = this.Name;
            sku.packaged_height = this.PackagedHeight;
            sku.packaged_length = this.PackagedLength;
            sku.packaged_width = this.PackagedWidth;
            sku.packaged_weight_kg = this.PackagedWeightKg;
            sku.measurement_unit = this.MeasurementUnit;
            sku.unit_multiplier = this.UnitMultiplier;
            sku.ref_id = this.RefId;
            sku.is_active = this.IsActive;

            return sku;
        }
    }
}
