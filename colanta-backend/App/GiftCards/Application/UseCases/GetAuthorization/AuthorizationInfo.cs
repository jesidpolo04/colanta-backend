namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    public class AuthorizationInfo
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public string date { get; set; }

        public void setFromTransactionAuthorization(TransactionAuthorization transactionAuthorization)
        {
            this.oid = transactionAuthorization.oid;
            this.value = transactionAuthorization.value;
            this.date = transactionAuthorization.date.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}
