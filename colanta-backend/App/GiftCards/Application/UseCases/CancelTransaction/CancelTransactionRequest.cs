namespace colanta_backend.App.GiftCards.Application
{
    public class CancelTransactionRequest
    {
        public decimal value { get; set; }
        public string requestId { get; set; }
    }
}
