namespace colanta_backend.App.CustomerCredit.Domain
{
    using System.Collections.Generic;
    using System;
    public interface IInvalidAccountsMail
    {
        void sendMail(List<InvalidAccountException> exceptions);
    }
}
