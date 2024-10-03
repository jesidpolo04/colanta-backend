namespace colanta_backend.App.OrderObservations.Infrastructure
{
    using App.OrderObservations.Domain;
    
    public class EFProductCutTypeValue {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ProductCutTypeValue GetProductCutType() {
            ProductCutTypeValue productObservationField = new(){
                Id = this.Id,
                Code = this.Code,
                Description = this.Description
            };
            return productObservationField;
        }

        public void SetEfProductCutType(ProductCutTypeValue productObservationField) {
            this.Id = productObservationField.Id;
            this.Code = productObservationField.Code;
            this.Description = productObservationField.Description;
        }
    }
}
