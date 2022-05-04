namespace colanta_backend.App.Products.Infraestructure
{
    public class CreateVtexSkuDto
    {
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

    }
}
