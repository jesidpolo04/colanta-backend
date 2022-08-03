namespace colanta_backend.App.Promotions.Domain
{
    using System.Collections.Generic;
    public interface IRenderPromotionsMail
    {
        void sendMail(List<Promotion> loadedPromotions, List<Promotion> incativatedPromotions, List<Promotion> failedPromotions);

    }
}
