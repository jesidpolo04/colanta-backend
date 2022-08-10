namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    public class VtexPromotionSummaryDto
    {
        public string idCalculatorConfiguration { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public bool isActive { get; set; }
        public PromotionSummary getPromotionSummary()
        {
            return new PromotionSummary(this.idCalculatorConfiguration, this.name, this.type, this.isActive);
        }
    }

}
