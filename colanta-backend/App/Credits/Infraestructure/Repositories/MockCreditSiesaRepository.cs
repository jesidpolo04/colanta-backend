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
                var code = this.generateCode();
                giftcard.siesa_id = code;
                giftcard.code = code;
                giftcard.token = code;
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

        private string generateCode()
        {
            string chars = "ABCDEFGHIJQLMNOPQRZT123456789";
            int codeLength = 5;
            string prefix = "MIDDLEWARE";
            string code = prefix;
            for(int i = 0; i < codeLength; i++)
            {
                Random random = new Random();
                code+= chars[random.Next(0, (codeLength - 1))];
            }
            return code;
        }
    }
}
