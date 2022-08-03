using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using CustomerCredit.Domain;
    public class RenderCreditAccountMailModel : PageModel
    {
        public string title = "Renderizado de Cupos";
        public List<CreditAccount> loadedAccounts;
        public List<CreditAccount> updatedAccounts;
        public List<CreditAccount> failedAccounts;
        public DateTime dateTime;

        public RenderCreditAccountMailModel(List<CreditAccount> loadedAccounts, List<CreditAccount> updatedAccounts, List<CreditAccount> failedAccounts)
        {
            
            this.loadedAccounts = loadedAccounts;
            this.updatedAccounts = updatedAccounts;
            this.failedAccounts = failedAccounts;
            this.dateTime = DateTime.Now;
        }

        public void OnGet()
        {
        }
    }
}
