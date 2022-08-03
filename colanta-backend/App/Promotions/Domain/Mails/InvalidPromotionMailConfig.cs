namespace colanta_backend.App.Promotions.Domain
{
    using System.Collections.Generic;
    public class InvalidPromotionMailConfig
    {
        public List<string> invalidBrands;
        public List<string> invalidCategories;
        public List<string> invalidProducts;
        public List<string> invalidSkus;
        public List<string> invalidGifts;
        public List<string> invalidList1Skus;
        public List<string> invalidList2Skus;

        public InvalidPromotionMailConfig
            (
            List<string> invalidBrands, 
            List<string> invalidCategories, 
            List<string> invalidProducts, 
            List<string> invalidSkus, 
            List<string> invalidGifts, 
            List<string> invalidList1Skus, 
            List<string> invalidList2Skus)
        {
            this.invalidBrands = invalidBrands;
            this.invalidCategories = invalidCategories;
            this.invalidProducts = invalidProducts;
            this.invalidSkus = invalidSkus;
            this.invalidGifts = invalidGifts;
            this.invalidList1Skus = invalidList1Skus;
            this.invalidList2Skus = invalidList2Skus;
        }
    }
}
