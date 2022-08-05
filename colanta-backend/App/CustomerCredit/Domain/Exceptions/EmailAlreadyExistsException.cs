namespace colanta_backend.App.CustomerCredit.Domain
{
    using System;
    public class EmailAlreadyExistException : InvalidAccountException
    {
        public string email;
        public EmailAlreadyExistException(CreditAccount account, string message = "El email ya está asociado a una cuenta") : base(account, message)
        {
            this.email = this.account.email;
        }
    }
}
