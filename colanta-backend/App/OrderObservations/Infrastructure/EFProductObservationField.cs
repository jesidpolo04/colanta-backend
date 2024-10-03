namespace colanta_backend.App.OrderObservations.Infrastructure
{
    using App.OrderObservations.Domain;

    public class EFProductObservationField {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ProductObservationField GetProductObservationField() {
            ProductObservationField productObservationField = new (){
                Id = this.Id,
                Code = this.Code,
                Description = this.Description
            };
            return productObservationField;
        }

        public void SetEfProductObservationField(ProductObservationField productObservationField) {
            this.Id = productObservationField.Id;
            this.Code = productObservationField.Code;
            this.Description = productObservationField.Description;
        }
    }
}
