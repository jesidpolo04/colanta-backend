namespace colanta_backend.App.Promotions.Domain
{
    public class PromotionSummary
    {
        public string vtexId;
        public string name;
        public bool isActive;
        public string type;

        public PromotionSummary(string vtexId, string name, string type, bool isActive)
        {
            this.vtexId = vtexId;
            this.name = name;
            this.isActive = isActive;
            this.type = type;
        }
    }
}
