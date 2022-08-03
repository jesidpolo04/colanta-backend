namespace colanta_backend.App.CustomerCredit.Domain
{
    using System.Collections.Generic;
    public interface IRenderAccountsMail
    {
        public void sendMail(List<CreditAccount> loadedAccounts, List<CreditAccount> updatedAccounts, List<CreditAccount> failedAccounts);
    }
}
