namespace colanta_backend.App.Promotions.Domain { 
    using System.Threading.Tasks;
    public interface PromotionsSiesaRepository
    {
        Task<Promotion[]> getAllPromotions();
    }
}
