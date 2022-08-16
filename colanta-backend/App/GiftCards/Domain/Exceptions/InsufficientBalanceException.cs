namespace colanta_backend.App.GiftCards.Domain
{
    using System;
    public class InsufficientBalanceException : Exception
    {
        public decimal resultBalance;
        public InsufficientBalanceException
            (decimal resultBalance, string message = "La tarjeta no tiene el saldo suficiente") : base(message)
        {
            this.resultBalance = resultBalance;
        }
    }
}
