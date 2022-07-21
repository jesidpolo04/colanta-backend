namespace colanta_backend.App.GiftCards.Application
{
    using GiftCards.Domain;
    public class SettlementInfoDto
    {
        public string oid { get; set; }
        public decimal value { get; set; }
        public string date { get; set; }

        public void setFromTransactionSettlement(TransactionSettlement transactionSettlement)
        {
            this.oid = transactionSettlement.oid;
            this.value = transactionSettlement.value;
            this.date = transactionSettlement.date.ToUniversalTime().ToString();
        }
    }
}
