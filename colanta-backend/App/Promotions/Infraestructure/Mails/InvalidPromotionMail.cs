namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    using Shared.Domain;
    public class InvalidPromotionMail : IInvalidPromotionMail
    {
        private EmailSender emailSender;
        private string subject;
        private string template;

        public InvalidPromotionMail(EmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.template = "./App/Promotions/Infraestructure/Mails/InvalidPromotionMail.cshtml";
        }

        public void sendMail(Promotion promotion, InvalidPromotionMailConfig config)
        {
            this.subject = $"No fue posible crear la promoción {promotion.siesa_id}";
            InvalidPromotionMailModel model = new InvalidPromotionMailModel(promotion, config);
            this.emailSender.SendEmail(this.subject, this.template, model, EmailAddresses.Tech);
        }
    }
}
