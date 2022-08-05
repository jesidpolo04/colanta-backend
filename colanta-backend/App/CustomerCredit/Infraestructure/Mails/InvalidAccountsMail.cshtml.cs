using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.CustomerCredit.Infraestructure
{
    using System;
    using System.Collections.Generic;
    using CustomerCredit.Domain;
    public class InvalidAccountsMailModel : PageModel
    {
        public List<InvalidAccountException> exceptions;
        public DateTime dateTime = DateTime.Now;

        public InvalidAccountsMailModel(List<InvalidAccountException> exceptions)
        {
            this.exceptions = exceptions;
        }
        public void OnGet()
        {
        }
    }
}
