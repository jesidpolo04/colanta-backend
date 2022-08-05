namespace colanta_backend.App.CustomerCredit.Domain
{
    using System;
    public class EmailIsRequiredException : InvalidAccountException
    {
        public EmailIsRequiredException(CreditAccount account, string message = "El email es requerido para crear la cuenta") : base(account, message) 
        {
        }
    }
}
