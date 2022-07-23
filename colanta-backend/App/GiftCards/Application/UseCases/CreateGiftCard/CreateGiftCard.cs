namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    using System;
    public class CreateGiftcard
    {
        private GiftCardsRepository localRepository;
        private decimal defaultBalance = 80000;
        private int redemptionCodeParts = 4;
        private int partLenght = 4;

        public CreateGiftcard(GiftCardsRepository localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<GiftCard> Invoke(CreateGiftCardRequest request)
        {
            GiftCard giftcard = request.getGiftCard();
            giftcard.business = null;
            giftcard.siesa_id = this.generateIdentificator();
            giftcard.balance = this.defaultBalance;

            string code = this.generateRedemptionCode();
            giftcard.code = code;
            giftcard.token = code;
            giftcard = await this.localRepository.saveGiftCard(giftcard);
            return giftcard;
        }

        private string generateIdentificator()
        {
            return "middleware_" + Guid.NewGuid().ToString();
        }

        private string generateRedemptionCode()
        {
            string characters = "ABCDEFGHIJQLMNOPQRSTVWXYZ";
            string code = "";
            for(int codePart = 0; codePart < redemptionCodeParts; codePart++)
            {
                for(int character = 0; character < partLenght; character++)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, characters.Length - 1);
                    code+=characters[randomNumber];
                }
                if(codePart < (redemptionCodeParts - 1))
                {
                    code += "-";
                }
            }
            return code;
        }
    }
}
