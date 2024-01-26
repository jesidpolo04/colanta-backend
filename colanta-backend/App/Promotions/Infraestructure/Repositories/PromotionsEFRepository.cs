using colanta_backend.App.Promotions.Domain;
using System.Threading.Tasks;

namespace colanta_backend.App.Promotions.Infraestructure
{
    using System.Text.Json;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using App.Shared.Infraestructure;
    using App.Promotions.Domain;
    using App.Brands.Domain;
    using App.Brands.Infraestructure;
    using App.Categories.Domain;
    using App.Categories.Infraestructure;
    using App.Products.Domain;
    using App.Products.Infraestructure;
    public class PromotionsEFRepository : Domain.PromotionsRepository
    {
        private ColantaContext dbContext;

        public PromotionsEFRepository(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }

        public Promotion[] getActivePromotions()
        {
            return dbContext.Promotions.Where( promotion => promotion.is_active == true )
                .ToList()
                .Select( promotion => addRelationsToPromotion(promotion).Result )
                .ToArray();
        }

        public async Task<Promotion[]> getAllPromotions()
        {
            EFPromotion[] efPromotions = this.dbContext.Promotions.ToArray();
            List<Promotion> promotions = new List<Promotion>();
            foreach(EFPromotion efPromotion in efPromotions)
            {
                string[] brands_ids = JsonSerializer.Deserialize<string[]>(efPromotion.brands_ids);
                EFBrand[] efBrands = this.dbContext.Brands.Where(brand => brands_ids.Contains(brand.id_siesa)).ToArray();
                List<Brand> brands = new List<Brand>();
                foreach (EFBrand efBrand in efBrands)
                {
                    brands.Add(efBrand.getBrandFromEFBrand());
                }

                string[] categories_ids = JsonSerializer.Deserialize<string[]>(efPromotion.categories_ids);
                EFCategory[] efCategories = this.dbContext.Categories.Where(category => categories_ids.Contains(category.siesa_id)).ToArray();
                List<Category> categories = new List<Category>();
                foreach (EFCategory efCategory in efCategories)
                {
                    categories.Add(efCategory.getCategoryFromEFCategory());
                }

                string[] products_ids = JsonSerializer.Deserialize<string[]>(efPromotion.products_ids);
                EFProduct[] efProducts = this.dbContext.Products.Where(product => products_ids.Contains(product.siesa_id)).ToArray();
                List<Product> products = new List<Product>();
                foreach (EFProduct efProduct in efProducts)
                {
                    products.Add(efProduct.getProductFromEfProduct());
                }

                string[] skus_ids = JsonSerializer.Deserialize<string[]>(efPromotion.skus_ids);
                EFSku[] efSkus = this.dbContext.Skus.Where(sku => skus_ids.Contains(sku.siesa_id)).ToArray();
                List<Sku> skus = new List<Sku>();
                foreach (EFSku efSku in efSkus)
                {
                    skus.Add(efSku.GetSkuFromEfSku());
                }

                string[] gifts_ids = JsonSerializer.Deserialize<string[]>(efPromotion.gifts_ids);
                EFSku[] efGifts = this.dbContext.Skus.Where(sku => gifts_ids.Contains(sku.concat_siesa_id)).ToArray();
                List<Sku> gifts = new List<Sku>();
                foreach (EFSku efGift in efGifts)
                {
                    gifts.Add(efGift.GetSkuFromEfSku());
                }

                string[] list_sku_1_buy_together_ids = JsonSerializer.Deserialize<string[]>(efPromotion.list_sku_1_buy_together_ids);
                EFSku[] efListSku1 = this.dbContext.Skus.Where(sku => list_sku_1_buy_together_ids.Contains(sku.concat_siesa_id)).ToArray();
                List<Sku> listSku1 = new List<Sku>(); ;
                foreach (EFSku efSku in efListSku1)
                {
                    listSku1.Add(efSku.GetSkuFromEfSku());
                }

                string[] list_sku_2_buy_together_ids = JsonSerializer.Deserialize<string[]>(efPromotion.list_sku_2_buy_together_ids);
                EFSku[] efListSku2 = this.dbContext.Skus.Where(sku => list_sku_2_buy_together_ids.Contains(sku.concat_siesa_id)).ToArray();
                List<Sku> listSku2 = new List<Sku>();
                foreach (EFSku efSku in efListSku2)
                {
                    listSku2.Add(efSku.GetSkuFromEfSku());
                }

                Promotion promotion = efPromotion.getPromotionFromEfPromotion();
                promotion.products = products.ToArray();
                promotion.skus = skus.ToArray();
                promotion.categories = categories.ToArray();
                promotion.brands = brands.ToArray();
                promotion.gifts = gifts.ToArray();
                promotion.list_sku_1_buy_together = listSku1.ToArray();
                promotion.list_sku_2_buy_together = listSku2.ToArray();
                promotions.Add(promotion);
            }
            return promotions.ToArray();
        }

        public async Task<Promotion[]> getDeltaPromotions(Promotion[] currentPromotions)
        {
            List<string> currentPromotionsIds = new List<string>();
            List<Promotion> deltaPromotions = new List<Promotion>(); 
            foreach (Promotion currentPromotion in currentPromotions)
            {
                currentPromotionsIds.Add(currentPromotion.siesa_id);
            }
            EFPromotion[] efDeltaPromotions = this.dbContext.Promotions
                .Where(promotion => !currentPromotionsIds.Contains(promotion.siesa_id) && promotion.is_active == true).ToArray();
            foreach(EFPromotion efDeltaPromotion in efDeltaPromotions)
            {
                deltaPromotions.Add(await this.addRelationsToPromotion(efDeltaPromotion));
            }
            return deltaPromotions.ToArray();
        }

        public async Task<Promotion> getPromotionBySiesaId(string siesaId)
        {
            var efPromotions = this.dbContext.Promotions.Where(promotion => promotion.siesa_id == siesaId);
            if(efPromotions.ToArray().Length > 0)
            {
                EFPromotion efPromotion = efPromotions.First();

                string[] brands_ids = JsonSerializer.Deserialize<string[]>(efPromotion.brands_ids);
                EFBrand[] efBrands = this.dbContext.Brands.Where(brand => brands_ids.Contains(brand.id_siesa)).ToArray();
                List<Brand> brands = new List<Brand>();
                foreach(EFBrand efBrand in efBrands)
                {
                    brands.Add(efBrand.getBrandFromEFBrand());
                }

                string[] categories_ids = JsonSerializer.Deserialize<string[]>(efPromotion.categories_ids);
                EFCategory[] efCategories = this.dbContext.Categories.Where(category => categories_ids.Contains(category.siesa_id)).ToArray();
                List<Category> categories = new List<Category>();
                foreach(EFCategory efCategory in efCategories)
                {
                    categories.Add(efCategory.getCategoryFromEFCategory());
                }

                string[] products_ids = JsonSerializer.Deserialize<string[]>(efPromotion.products_ids);
                EFProduct[] efProducts = this.dbContext.Products.Where(product => products_ids.Contains(product.siesa_id)).ToArray();
                List<Product> products = new List<Product>();
                foreach(EFProduct efProduct in efProducts)
                {
                    products.Add(efProduct.getProductFromEfProduct());
                }

                string[] skus_ids = JsonSerializer.Deserialize<string[]>(efPromotion.skus_ids);
                EFSku[] efSkus = this.dbContext.Skus.Where(sku => skus_ids.Contains(sku.siesa_id)).ToArray();
                List<Sku> skus = new List<Sku>();
                foreach(EFSku efSku in efSkus)
                {
                    skus.Add(efSku.GetSkuFromEfSku());
                }

                string[] gifts_ids = JsonSerializer.Deserialize<string[]>(efPromotion.gifts_ids);
                EFSku[] efGifts = this.dbContext.Skus.Where(sku => gifts_ids.Contains(sku.concat_siesa_id)).ToArray();
                List<Sku> gifts = new List<Sku>();
                foreach(EFSku efGift in efGifts)
                {
                    gifts.Add(efGift.GetSkuFromEfSku());
                }
                
                string[] list_sku_1_buy_together_ids = JsonSerializer.Deserialize<string[]>(efPromotion.list_sku_1_buy_together_ids);
                EFSku[] efListSku1 = this.dbContext.Skus.Where(sku => list_sku_1_buy_together_ids.Contains(sku.concat_siesa_id)).ToArray();
                List<Sku> listSku1 = new List<Sku>();
                foreach(EFSku efSku in efListSku1)
                {
                    listSku1.Add(efSku.GetSkuFromEfSku());
                }
             
                string[] list_sku_2_buy_together_ids = JsonSerializer.Deserialize<string[]>(efPromotion.list_sku_2_buy_together_ids);
                EFSku[] efListSku2 = this.dbContext.Skus.Where(sku => list_sku_2_buy_together_ids.Contains(sku.concat_siesa_id)).ToArray();
                List<Sku> listSku2 = new List<Sku>();
                foreach(EFSku efSku in efListSku2)
                {
                    listSku2.Add(efSku.GetSkuFromEfSku());
                }

                Promotion promotion = efPromotion.getPromotionFromEfPromotion();
                promotion.products = products.ToArray();
                promotion.skus = skus.ToArray();
                promotion.categories = categories.ToArray();
                promotion.brands = brands.ToArray();
                promotion.gifts = gifts.ToArray();
                promotion.list_sku_1_buy_together = listSku1.ToArray();
                promotion.list_sku_2_buy_together = listSku2.ToArray();

                return promotion;
            }
            return null;
        }

        public async Task<Promotion> getPromotionByVtexId(string vtexId)
        {
            EFPromotion[] efPromotions = this.dbContext.Promotions.Where(promotion => promotion.vtex_id == vtexId).ToArray();
            if (efPromotions.Length > 0)
            {
                return await this.addRelationsToPromotion(efPromotions.First());
            }
            return null;
        }

        public async Task<Promotion[]> getVtexNullPromotions()
        {
            EFPromotion[] efPromotions = this.dbContext.Promotions.Where(promotion => promotion.vtex_id == null).ToArray();
            List<Promotion> promotions = new List<Promotion>();
            foreach(EFPromotion efPromotion in efPromotions)
            {
                promotions.Add(efPromotion.getPromotionFromEfPromotion());
            }
            return promotions.ToArray();
        }

        public async Task<Promotion[]> getVtexPromotions()
        {
            return dbContext.Promotions
            .Where(promotion => promotion.vtex_id != null)
            .ToList()
            .Select( efPromotion => addRelationsToPromotion(efPromotion).Result).ToArray();
        }

        public async Task<Promotion> savePromotion(Promotion promotion)
        {
            EFPromotion efPromotion = new EFPromotion();
            efPromotion.setEfPromotionFromPromotion(promotion);
            this.dbContext.Add(efPromotion);
            this.dbContext.SaveChanges();
            return await this.getPromotionBySiesaId(promotion.siesa_id);
        }

        public async Task<Promotion> updatePromotion(Promotion promotion)
        {
            EFPromotion efPromotion = this.dbContext.Promotions.Find(promotion.id);
            efPromotion.setEfPromotionFromPromotion(promotion);
            this.dbContext.SaveChanges();
            return promotion;
        }

        private async Task<Promotion> addRelationsToPromotion(EFPromotion efPromotion)
        {
            string[] brands_ids = JsonSerializer.Deserialize<string[]>(efPromotion.brands_ids);
            EFBrand[] efBrands = this.dbContext.Brands.Where(brand => brands_ids.Contains(brand.id_siesa)).ToArray();
            List<Brand> brands = new List<Brand>();
            foreach (EFBrand efBrand in efBrands)
            {
                brands.Add(efBrand.getBrandFromEFBrand());
            }

            string[] categories_ids = JsonSerializer.Deserialize<string[]>(efPromotion.categories_ids);
            EFCategory[] efCategories = this.dbContext.Categories.Where(category => categories_ids.Contains(category.siesa_id)).ToArray();
            List<Category> categories = new List<Category>();
            foreach (EFCategory efCategory in efCategories)
            {
                categories.Add(efCategory.getCategoryFromEFCategory());
            }

            string[] products_ids = JsonSerializer.Deserialize<string[]>(efPromotion.products_ids);
            EFProduct[] efProducts = this.dbContext.Products.Where(product => products_ids.Contains(product.siesa_id)).ToArray();
            List<Product> products = new List<Product>();
            foreach (EFProduct efProduct in efProducts)
            {
                products.Add(efProduct.getProductFromEfProduct());
            }

            string[] skus_ids = JsonSerializer.Deserialize<string[]>(efPromotion.skus_ids);
            EFSku[] efSkus = this.dbContext.Skus.Where(sku => skus_ids.Contains(sku.siesa_id)).ToArray();
            List<Sku> skus = new List<Sku>();
            foreach (EFSku efSku in efSkus)
            {
                skus.Add(efSku.GetSkuFromEfSku());
            }

            string[] gifts_ids = JsonSerializer.Deserialize<string[]>(efPromotion.gifts_ids);
            EFSku[] efGifts = this.dbContext.Skus.Where(sku => gifts_ids.Contains(sku.siesa_id)).ToArray();
            List<Sku> gifts = new List<Sku>();
            foreach (EFSku efGift in efGifts)
            {
                gifts.Add(efGift.GetSkuFromEfSku());
            }

            string[] list_sku_1_buy_together_ids = JsonSerializer.Deserialize<string[]>(efPromotion.list_sku_1_buy_together_ids);
            EFSku[] efListSku1 = this.dbContext.Skus.Where(sku => list_sku_1_buy_together_ids.Contains(sku.siesa_id)).ToArray();
            List<Sku> listSku1 = new List<Sku>();
            foreach (EFSku efSku in efListSku1)
            {
                listSku1.Add(efSku.GetSkuFromEfSku());
            }

            string[] list_sku_2_buy_together_ids = JsonSerializer.Deserialize<string[]>(efPromotion.list_sku_2_buy_together_ids);
            EFSku[] efListSku2 = this.dbContext.Skus.Where(sku => list_sku_2_buy_together_ids.Contains(sku.siesa_id)).ToArray();
            List<Sku> listSku2 = new List<Sku>();
            foreach (EFSku efSku in efListSku2)
            {
                listSku2.Add(efSku.GetSkuFromEfSku());
            }

            Promotion promotion = efPromotion.getPromotionFromEfPromotion();
            promotion.products = products.ToArray();
            promotion.skus = skus.ToArray();
            promotion.categories = categories.ToArray();
            promotion.brands = brands.ToArray();
            promotion.gifts = gifts.ToArray();
            promotion.list_sku_1_buy_together = listSku1.ToArray();
            promotion.list_sku_2_buy_together = listSku2.ToArray();

            return promotion;
        }
    }
}
