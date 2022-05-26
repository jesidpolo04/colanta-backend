namespace colanta_backend.App.GiftCards.Application
{
    using System.Threading.Tasks;
    using GiftCards.Domain;
    public class ListAllGiftCardByDocumentAndBusiness
    {
        private GiftCardsSiesaRepository siesaRepository;
        public ListAllGiftCardByDocumentAndBusiness(GiftCardsSiesaRepository siesaRepository)
        {
            this.siesaRepository = siesaRepository;
        }

        public async Task<GiftCard[]> Invoke(string document, string business)
        {
            return await this.siesaRepository.getGiftCardsByDocumentAndBusiness(document, business);
        }
    }
}
