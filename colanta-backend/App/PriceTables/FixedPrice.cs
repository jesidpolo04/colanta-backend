using System;

namespace colanta_backend.App.PriceTables{
    public class FixedPrice{
        public int Id { get; set; }
        public decimal Value { get; set; }
        public decimal ListPrice { get; set; }
        public decimal VtexSkuId { get; set; }
        public string PriceTableName { get; set; }
        public PriceTable PriceTable { get; set; } 
        public int MinQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}