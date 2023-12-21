namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using Shared.Domain;
    using System.Threading.Tasks;
    using Shared.Domain;
    using Shared.Application;
    using Brands.Domain;
    using Categories.Domain;
    using Products.Domain;
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Collections.Generic;
    using colanta_backend.App.PriceTables;
    using colanta_backend.App.Prices.Domain;
    using System.Linq;
    using System.Threading;
    using colanta_backend.App.Shared;

    public class RenderPromotions : IDisposable
    {
        private string processName = "Renderizado de promociones";
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;
        private PromotionsSiesaRepository siesaRepository;
        private IProcess process;
        private ILogger logger;
        private BrandsRepository brandsRepository;
        private CategoriesRepository categoriesRepository;
        private ProductsRepository productsRepository;
        private SkusRepository skuRepository;
        private PricesRepository priceLocalRepository;
        private PriceTablesRepository priceTableRepository;
        private PriceTablesVtexService priceTableVtexService;
        private IRenderPromotionsMail renderPromotionsMail;
        private IInvalidPromotionMail invalidPromotionMail;


        private List<Promotion> loadPromotions = new List<Promotion>();
        private List<Promotion> inactivePromotions = new List<Promotion>();
        private List<Promotion> inactivatedPromotions = new List<Promotion>();
        private List<Promotion> failedPromotions = new List<Promotion>();
        private List<Promotion> notProccecedPromotions = new List<Promotion>();


        private CustomConsole console = new CustomConsole();
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions();

        public RenderPromotions(
                PromotionsRepository localRepository,
                PromotionsVtexRepository vtexRepository,
                PromotionsSiesaRepository siesaRepository,
                BrandsRepository brandsRepository,
                CategoriesRepository categoriesRepository,
                ProductsRepository productsRepository,
                SkusRepository skuRepository,
                PricesRepository priceLocalRepository,
                PriceTablesRepository priceTableRepository,
                PriceTablesVtexService priceTableVtexService,
                IProcess process,
                ILogger logger,
                IRenderPromotionsMail renderPromotionsMail,
                IInvalidPromotionMail invalidPromotionMail
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.brandsRepository = brandsRepository;
            this.categoriesRepository = categoriesRepository;
            this.productsRepository = productsRepository;
            this.skuRepository = skuRepository;
            this.priceLocalRepository = priceLocalRepository;
            this.priceTableRepository = priceTableRepository;
            this.priceTableVtexService = priceTableVtexService;
            this.renderPromotionsMail = renderPromotionsMail;
            this.invalidPromotionMail = invalidPromotionMail;
            this.process = process;
            this.logger = logger;
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public async Task Invoke()
        {
            this.console.processStartsAt(processName, DateTime.Now);
            try
            {
                Promotion[] allSiesaPromotions = await this.siesaRepository.getAllPromotions();
                Promotion[] deltaPromotions = await this.localRepository.getDeltaPromotions(allSiesaPromotions);
                inactiveDeltaPromotions(deltaPromotions);

                foreach (Promotion siesaPromotion in allSiesaPromotions)
                {
                    try
                    {
                        Promotion? localPromotion = await this.localRepository.getPromotionBySiesaId(siesaPromotion.siesa_id);

                        if (localPromotion != null)
                        {
                            if (localPromotion.is_active == true)
                            {
                                this.notProccecedPromotions.Add(localPromotion);
                            }
                            if (localPromotion.is_active == false && localPromotion.vtex_id != null)
                            {
                                this.inactivePromotions.Add(localPromotion);
                            }
                        }

                        if (localPromotion == null)
                        {
                            if (!this.validPromotion(siesaPromotion))
                            {
                                this.notProccecedPromotions.Add(siesaPromotion);
                                continue;
                            }
                            localPromotion = await this.localRepository.savePromotion(siesaPromotion);
                            var percentualDiscountValue = localPromotion.percentual_discount_value;
                            if (percentualDiscountValue != null && percentualDiscountValue > 0)
                            {
                                createPriceTable(localPromotion.price_table_name, localPromotion);
                            }
                            Promotion vtexPromotion = await vtexRepository.savePromotion(localPromotion);
                            localPromotion.vtex_id = vtexPromotion.vtex_id;
                            await this.localRepository.updatePromotion(localPromotion);

                            this.loadPromotions.Add(localPromotion);
                        }
                    }
                    catch (VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.failedPromotions.Add(siesaPromotion);
                        await this.logger.writelog(vtexException);
                    }
                    catch (Exception exception)
                    {
                        this.console.throwException(exception.Message);
                        await this.logger.writelog(exception);
                    }
                }
            }
            catch (SiesaException siesaException)
            {
                this.console.throwException(siesaException.Message);
                await this.logger.writelog(siesaException);
            }
            catch (Exception genericException)
            {
                this.console.throwException(genericException.Message);
                await this.logger.writelog(genericException);
            }

            this.renderPromotionsMail.sendMail(this.loadPromotions, this.inactivatedPromotions, this.failedPromotions);
            this.console.processEndstAt(processName, DateTime.Now);
        }

        private bool validPromotion(Promotion promotion)
        {
            bool validPromotion = true;

            List<string> inexistBrandsIds = this.invalidBrands(promotion.brands_ids);
            List<string> inexistCategoriesIds = this.invalidCategories(promotion.categories_ids);
            List<string> inexistSkusIds = this.invalidSkus(promotion.skus_ids);
            List<string> inexistProductsIds = this.invalidProducts(promotion.products_ids);
            List<string> inexistGiftsIds = this.invalidGifts(promotion.gifts_ids);
            List<string> inexistList1SkusIds = this.invalidListSkus(promotion.list_sku_1_buy_together_ids);
            List<string> inexistList2SkusIds = this.invalidListSkus(promotion.list_sku_2_buy_together_ids);

            validPromotion = inexistBrandsIds.Count > 0 ? false : validPromotion;
            validPromotion = inexistCategoriesIds.Count > 0 ? false : validPromotion;
            validPromotion = inexistProductsIds.Count > 0 ? false : validPromotion;
            validPromotion = inexistSkusIds.Count > 0 ? false : validPromotion;
            validPromotion = inexistGiftsIds.Count > 0 ? false : validPromotion;
            validPromotion = inexistList1SkusIds.Count > 0 ? false : validPromotion;
            validPromotion = inexistList2SkusIds.Count > 0 ? false : validPromotion;

            if (!validPromotion)
            {
                InvalidPromotionMailConfig mailConfig = new InvalidPromotionMailConfig(
                    inexistBrandsIds,
                    inexistCategoriesIds,
                    inexistProductsIds,
                    inexistSkusIds,
                    inexistGiftsIds,
                    inexistList1SkusIds,
                    inexistList2SkusIds
                    );
                this.invalidPromotionMail.sendMail(promotion, mailConfig);
                return validPromotion;
            }
            return validPromotion;
        }

        private List<string> invalidBrands(string brandsIds)
        {
            List<string> inexistBrandsIds = new List<string>();
            List<string> brandsIdsList = JsonSerializer.Deserialize<List<string>>(brandsIds);
            if (brandsIdsList.Count == 0) return inexistBrandsIds;
            foreach (string brandId in brandsIdsList)
            {
                Brand brand = this.brandsRepository.getBrandBySiesaId(brandId);
                if (brand == null)
                    inexistBrandsIds.Add(brandId);
            }
            return inexistBrandsIds;
        }

        private List<string> invalidCategories(string categoriesIds)
        {
            List<string> inexistCateories = new List<string>();
            List<string> categoriesIdsList = JsonSerializer.Deserialize<List<string>>(categoriesIds);
            if (categoriesIdsList.Count == 0) return inexistCateories;
            foreach (string categoryId in categoriesIdsList)
            {
                Category category = this.categoriesRepository.getCategoryBySiesaId(categoryId).Result;
                if (category == null)
                    inexistCateories.Add(categoryId);
            }
            return inexistCateories;
        }

        private List<string> invalidSkus(string skusIds)
        {
            List<string> inexistSkus = new List<string>();
            List<string> skusIdsList = JsonSerializer.Deserialize<List<string>>(skusIds);
            if (skusIdsList.Count == 0) return inexistSkus;
            foreach (string skuId in skusIdsList)
            {
                Sku sku = this.skuRepository.getSkuBySiesaId(skuId).Result;
                if (sku == null)
                    inexistSkus.Add(skuId);
            }
            return inexistSkus;
        }

        private List<string> invalidProducts(string productsIds)
        {
            List<string> inexistProducts = new List<string>();
            List<string> productsIdsList = JsonSerializer.Deserialize<List<string>>(productsIds);
            if (productsIdsList.Count == 0) return inexistProducts;
            foreach (string productId in productsIdsList)
            {
                Product product = this.productsRepository.getProductBySiesaId(productId).Result;
                if (product == null)
                    inexistProducts.Add(productId);
            }
            return inexistProducts;
        }

        private List<string> invalidGifts(string giftsConcatSiesaIds)
        {
            List<string> inexistSkusIds = new List<string>();
            List<string> giftConcatSiesaIdsList = JsonSerializer.Deserialize<List<string>>(giftsConcatSiesaIds);
            if (giftConcatSiesaIdsList.Count == 0) return inexistSkusIds;
            foreach (string giftConcatSiesaId in giftConcatSiesaIdsList)
            {
                Sku gift = this.skuRepository.getSkuByConcatSiesaId(giftConcatSiesaId).Result;
                if (gift == null)
                    inexistSkusIds.Add(giftConcatSiesaId);
            }
            return inexistSkusIds;
        }

        private List<string> invalidListSkus(string listConcatSiesaIds)
        {
            List<string> inexistSkusIds = new List<string>();
            List<string> listConcatSiesaIdsList = JsonSerializer.Deserialize<List<string>>(listConcatSiesaIds);
            if (listConcatSiesaIdsList.Count == 0) return inexistSkusIds;
            foreach (string skuConcatSiesaId in listConcatSiesaIdsList)
            {
                Sku sku = this.skuRepository.getSkuByConcatSiesaId(skuConcatSiesaId).Result;
                if (sku == null)
                    inexistSkusIds.Add(skuConcatSiesaId);
            }
            return inexistSkusIds;
        }

        private async Task inactiveDeltaPromotions(Promotion[] deltaPromotions)
        {
            foreach (Promotion deltaPromotion in deltaPromotions)
            {
                try
                {
                    deltaPromotion.is_active = false;
                    await vtexRepository.changePromotionState(deltaPromotion.vtex_id, false);
                    await localRepository.updatePromotion(deltaPromotion);
                    this.inactivatedPromotions.Add(deltaPromotion);
                }
                catch (VtexException vtexException)
                {
                    this.console.throwException(vtexException.Message);
                    await this.logger.writelog(vtexException);
                }
            }
        }

        void createPriceTable(string priceTableName, Promotion promotion)
        {
            var table = priceTableRepository.GetByName(priceTableName);
            if (table == null)
            {
                var priceTable = new PriceTable
                {
                    Name = priceTableName
                };
                priceTableRepository.Save(priceTable);
            }
            var fixedPrices = createFixedPrices(table, promotion);
            List<Task> responses = new List<Task>();
            foreach (var fixedPrice in fixedPrices)
            {
                var response = priceTableVtexService.AddFixedPriceToPriceTable(fixedPrice);
                responses.Add(response);
                Thread.Sleep(50);
            }
            Task.WhenAll(responses).Wait();
        }

        List<FixedPrice> createFixedPrices(PriceTable priceTable, Promotion promotion)
        {
            var fixedPrices = new List<FixedPrice>();

            foreach (var brand in promotion.brands)
            {
                var prices = priceLocalRepository.getPricesByBrand((int)brand.id);
                foreach (var price in prices)
                {
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        ListPrice = price.price,
                        Value = price.price - (price.base_price * (promotion.percentual_discount_value / 100)),
                        MinQuantity = 1,
                        VtexSkuId = price.sku_id
                    });
                }
            }

            foreach (var category in promotion.categories)
            {
                var prices = priceLocalRepository.getPricesByCategory((int)category.id);
                foreach (var price in prices)
                {
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        ListPrice = price.price,
                        Value = price.price - (price.base_price * (promotion.percentual_discount_value / 100)),
                        MinQuantity = 1,
                        VtexSkuId = price.sku_id
                    });
                }

            }

            foreach (var product in promotion.products)
            {
                var prices = priceLocalRepository.getPricesByProduct((int)product.id);
                foreach (var price in prices)
                {
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        ListPrice = price.price,
                        Value = price.price - (price.base_price * (promotion.percentual_discount_value / 100)),
                        MinQuantity = 1,
                        VtexSkuId = price.sku_id
                    });
                }

            }

            foreach (var sku in promotion.skus)
            {
                var skuIds = promotion.skus.Select(sku => (int)sku.id);
                var prices = priceLocalRepository.getPricesBySkuIds(skuIds.ToArray());
                foreach (var price in prices)
                {
                    fixedPrices.Add(new FixedPrice
                    {
                        PriceTable = priceTable,
                        ListPrice = price.price,
                        Value = price.price - (price.base_price * (promotion.percentual_discount_value / 100)),
                        MinQuantity = 1,
                        VtexSkuId = price.sku_id
                    });
                }
            }

            return fixedPrices;
        }

        public void Dispose()
        {
            this.loadPromotions.Clear();
            this.inactivatedPromotions.Clear();
            this.failedPromotions.Clear();
            this.notProccecedPromotions.Clear();
            this.inactivePromotions.Clear();
        }
    }
}
