namespace colanta_backend.App.Promotions.Domain { 
    using System.Threading.Tasks;
    public interface PromotionsRepository
    {
        public Task<Promotion> getPromotionBySiesaId(string siesaId);
        public Task<Promotion> getPromotionByVtexId(string vtexId);
        public Task<Promotion[]> getDeltaPromotions(Promotion[] currentPromotions);
        public Task<Promotion[]> getVtexNullPromotions();
        public Task<Promotion[]> getVtexPromotions();
        public Task<Promotion> savePromotion(Promotion promotion);
        public Task<Promotion[]> getAllPromotions();
        public Task<Promotion> updatePromotion(Promotion promotion);
        public Promotion[] getActivePromotions();
    }
}
