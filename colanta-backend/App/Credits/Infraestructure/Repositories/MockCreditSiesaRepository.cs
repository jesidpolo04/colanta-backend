namespace colanta_backend.App.Credits.Infraestructure
{
    using colanta_backend.App.GiftCards.Domain;
    using Credits.Domain;
    using GiftCards.Domain;
    using Shared.Domain;
    using System;
    using System.Threading.Tasks;

    public class MockCreditSiesaRepository : Domain.CreditsSiesaRepository
    {
        public async Task<GiftCard> getCreditByDocumentAndEmail(string document, string email)
        {
            var documentAvailable = "1002999476";
            var emailAvailable = "jesing482@gmail.com";
            if(document == documentAvailable && email == emailAvailable)
            {
                var giftcard = new GiftCard();
                giftcard.siesa_id = "BGV1234MIDDLEWARE";
                giftcard.code = "BGV1234MIDDLEWARE";
                giftcard.token = "BGV1234MIDDLEWARE";
                giftcard.owner = documentAvailable;
                giftcard.owner_email = emailAvailable;
                giftcard.provider = Providers.CUPO;
                giftcard.balance = 32000;
                giftcard.emision_date = DateTime.Now.ToString(DateFormats.UTC);
                giftcard.expire_date = DateTime.Now.AddMinutes(5).ToString(DateFormats.UTC);
                giftcard.name = "Cupo Lacteo";
                giftcard.used = false;
                return giftcard;
            }
            return null;
        }
    }
}
