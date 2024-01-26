using System;

namespace colanta_backend.App.PriceTables{
    public class PriceTable{
        public string Name { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}