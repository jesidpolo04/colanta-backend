using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Promotions.Infraestructure
{
    using System.Collections.Generic;
    using System;
    using Promotions.Domain;
    public class RenderPromotionsMailModel : PageModel
    {
        public List<Promotion> loadedPromotions;
        public List<Promotion> inactivatedPromotions;
        public List<Promotion> failedPromotions;
        public DateTime dateTime;

        public RenderPromotionsMailModel(List<Promotion> loadedPromotions, List<Promotion> inactivatedPromotions, List<Promotion> failedPromotions)
        {
            this.loadedPromotions = loadedPromotions;
            this.inactivatedPromotions = inactivatedPromotions;
            this.failedPromotions = failedPromotions;
            this.dateTime = DateTime.Now;
        }

        public void OnGet()
        {

        }
    }
}
