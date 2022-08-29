namespace colanta_backend.App.GiftCards.Domain
{
    using System;
    public class GiftCard
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string token { get; set; }
        public string provider { get; set; }
        public string business { get; set; }
        public decimal balance { get; set; }
        public string owner { get; set; }
        public string owner_email { get; set; }
        public bool used { get; set; }
        public string emision_date { get; set; }
        public string expire_date { get; set; }

        public Transaction newTransaction(decimal value, string requestJson)
        {
            decimal resultBalance = balance - value;
            if (value < 0) throw new InvalidOperationException("El valor de la transacción no puede ser negativo");
            if (resultBalance < 0) throw new InsufficientBalanceException(resultBalance);
            var transaction = new Transaction(this, value, requestJson);
            this.balance -= value;
            return transaction;
        }

        public void updateBalance(decimal newBalance)
        {
            this.balance = newBalance;
        }

        public void hasBeenUsed()
        {
            this.used = true;
        }
    }
}
