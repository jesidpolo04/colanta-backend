using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    using System;
    public class InvalidPromotionMailModel : PageModel
    {
        public Promotion promotion;
        public InvalidPromotionMailConfig config;
        public DateTime dateTime;

        public InvalidPromotionMailModel(Promotion promotion, InvalidPromotionMailConfig config)
        {
            this.promotion = promotion;
            this.config = config;
            this.dateTime = DateTime.Now;
        }

        public void OnGet()
        {
        }
    }
}
