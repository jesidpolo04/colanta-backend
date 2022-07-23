namespace colanta_backend.App.GiftCards.Application
{
    using System;
    using GiftCards.Domain;
    public class CreateGiftCardRequest
    {
        public string relationName { get; set; }
        public DateTime emissionDate { get; set; }
        public DateTime expiringDate { get; set; }
        public string caption { get; set; }
        public bool restrictedToOwner { get; set; }
        public bool multipleRedemptions { get; set; }
        public bool multipleCredits { get; set; }
        public string profileId { get; set; }

        public GiftCard getGiftCard()
        {
            GiftCard giftCard = new GiftCard();

            giftCard.name = relationName;
            giftCard.emision_date = emissionDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            giftCard.expire_date = expiringDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            giftCard.owner = profileId;
            return giftCard;
        }
    }
}
