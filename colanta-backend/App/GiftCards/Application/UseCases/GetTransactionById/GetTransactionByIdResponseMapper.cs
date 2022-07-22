namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    using System.Text.Json;
    public class GetTransactionByIdResponseMapper
    {
        public static GetTransactionByIdResponseDto getDto(Transaction transaction)
        {
            var dto = JsonSerializer.Deserialize<GetTransactionByIdResponseDto>(transaction.json);
            var authorization = new GetTransactionByIdResponseAuthorizationDto();
            var settlement = new GetTransactionByIdResponseSettlementDto();
            var cancellation = new GetTransactionByIdResponseCancellationDto();

            authorization.href = $"/giftcards/{transaction.card.siesa_id}/transactions/{transaction.id}/authorization";
            settlement.href = $"/giftcards/{transaction.card.siesa_id}/transactions/{transaction.id}/settlements";
            cancellation.href = $"/giftcards/{transaction.card.siesa_id}/transactions/{transaction.id}/cancellations";

            dto.date = transaction.date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            dto.authorization = authorization;
            dto.settlement = settlement;
            dto.cancellation = cancellation;

            return dto;
        }
    }
}
