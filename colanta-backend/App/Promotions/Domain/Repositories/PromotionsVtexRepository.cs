namespace colanta_backend.App.Promotions.Domain
{
    using System.Threading.Tasks;
    public interface PromotionsVtexRepository
    {
        public void changeEnvironment(string environment);
        public Task<Promotion> getPromotionByVtexId(string vtexId, string environment);
        public Task<PromotionSummary[]> getPromotionsList();
        public Task<Promotion> savePromotion(Promotion promotion);
        public Task<Promotion> updatePromotion(Promotion promotion);
    }
}
