namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Threading.Tasks;
    using System.Text.Json;
    public class CreateGiftCardTransaction
    {
        private GiftCardsRepository localRepository;

        public CreateGiftCardTransaction(GiftCardsRepository giftCardsRepository)
        {
            this.localRepository = giftCardsRepository;
        }

        public async Task<TransactionSummaryDto> Invoke(string giftcardId, CreateGiftCardTransactionDto request)
        {
            GiftCard giftcard = await this.localRepository.getGiftCardBySiesaId(giftcardId);
            Transaction transaction = new Transaction(giftcard, request.value, JsonSerializer.Serialize(request));
            transaction = await this.localRepository.saveGiftCardTransaction(transaction);
            return new TransactionSummaryDto(giftcardId, transaction.id);
        }
    }
}
