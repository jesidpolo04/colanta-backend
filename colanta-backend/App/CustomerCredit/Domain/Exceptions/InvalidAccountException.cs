namespace colanta_backend.App.CustomerCredit.Domain
{
    using System;
    public class InvalidAccountException : Exception
    {
        public CreditAccount account;
        public InvalidAccountException(CreditAccount account, string message) : base(message)
        {
            this.account = account;
        }
    }
}
