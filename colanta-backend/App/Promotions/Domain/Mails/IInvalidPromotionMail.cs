namespace colanta_backend.App.Promotions.Domain
{
    using System.Collections.Generic;
    public interface IInvalidPromotionMail
    {
        public void sendMail(Promotion promotion, InvalidPromotionMailConfig config);
    }
}
