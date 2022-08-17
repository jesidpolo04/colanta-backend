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
        private IRenderPromotionsMail renderPromotionsMail;
        private IInvalidPromotionMail invalidPromotionMail;


        private List<Promotion> loadPromotions = new List<Promotion>();
        private List<Promotion> inactivePromotions = new List<Promotion>();
        private List<Promotion> inactivatedPromotions = new List<Promotion>();
        private List<Promotion> failedPromotions = new List<Promotion>();
        private List<Promotion> notProccecedPromotions = new List<Promotion>();
        private int obtainedPromotions = 0;

        private List<Detail> details = new List<Detail>();

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
                this.details.Add(new Detail(
                            origin: "siesa",
                            action: "obtener todas las promociones",
                            content: JsonSerializer.Serialize(allSiesaPromotions, this.jsonOptions),
                            description: "petición para obtener las promociones, completada con éxito",
                            success: true));

                Promotion[] deltaPromotions = await this.localRepository.getDeltaPromotions(allSiesaPromotions);
                foreach (Promotion deltaPromotion in deltaPromotions)
                {
                    try
                    {
                        deltaPromotion.is_active = false;
                        vtexRepository.updatePromotion(deltaPromotion).Wait();
                        await localRepository.updatePromotion(deltaPromotion);
                        this.inactivatedPromotions.Add(deltaPromotion);
                        this.details.Add(new Detail(
                                origin: "vtex",
                                action: "actualizar promoción",
                                content: JsonSerializer.Serialize(deltaPromotion, this.jsonOptions),
                                description: "petición para actualizar la promoción, completada con éxito",
                                success: true));
                    }
                    catch(VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.details.Add(new Detail("vtex", vtexException.requestUrl, vtexException.responseBody, vtexException.Message, false));
                        await this.logger.writelog(vtexException);
                    }
                }
                foreach (Promotion siesaPromotion in allSiesaPromotions)
                {
                    try
                    {
                        this.obtainedPromotions++;

                        Promotion? localPromotion = await this.localRepository.getPromotionBySiesaId(siesaPromotion.siesa_id);

                        if (localPromotion != null)
                        {
                            if (localPromotion.is_active == true)
                            {
                                this.notProccecedPromotions.Add(localPromotion);
                            }
                            if (localPromotion.is_active == false && localPromotion.vtex_id !=  null)
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
                            Promotion vtexPromotion = await vtexRepository.savePromotion(localPromotion);
                            localPromotion.vtex_id = vtexPromotion.vtex_id;
                            await this.localRepository.updatePromotion(localPromotion);

                            this.loadPromotions.Add(localPromotion);
                            this.details.Add(new Detail(
                                origin: "vtex",
                                action: "crear o actualizar promoción",
                                content: JsonSerializer.Serialize(localPromotion, this.jsonOptions),
                                description: "petición para crear o actualizar promoción, completada con éxito",
                                success: true));
                        }
                    }
                    catch(VtexException vtexException)
                    {
                        this.console.throwException(vtexException.Message);
                        this.failedPromotions.Add(siesaPromotion);
                        this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: vtexException.requestUrl,
                                    content: vtexException.responseBody,
                                    description: vtexException.Message,
                                    success: false));
                        await this.logger.writelog(vtexException);
                    }
                }
            }
            catch(SiesaException siesaException)
            {
                this.console.throwException(siesaException.Message);
                this.details.Add(new Detail(
                            origin: "siesa",
                            action: siesaException.requestUrl,
                            content: siesaException.requestBody,
                            description: siesaException.Message,
                            success: false));
                await this.logger.writelog(siesaException);
            }
            catch(Exception genericException)
            {
                this.console.throwException(genericException.Message);
                await this.logger.writelog(genericException);
            }

            this.renderPromotionsMail.sendMail(this.loadPromotions, this.inactivatedPromotions, this.failedPromotions);
            this.process.Log(
                        name: processName,
                        total_loads: this.loadPromotions.Count,
                        total_errors: this.failedPromotions.Count,
                        total_not_procecced: this.notProccecedPromotions.Count,
                        total_obtained: this.obtainedPromotions,
                        json_details: JsonSerializer.Serialize(this.details, this.jsonOptions));
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
                //this.invalidPromotionMail.sendMail(promotion, mailConfig);
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
            foreach(string categoryId in categoriesIdsList)
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
                if(sku == null)
                    inexistSkus.Add(skuId);
            }
            return inexistSkus;
        }

        private List<string> invalidProducts(string productsIds)
        {
            List<string> inexistProducts = new List<string>();
            List<string> productsIdsList = JsonSerializer.Deserialize<List<string>>(productsIds);
            if(productsIdsList.Count == 0) return inexistProducts;
            foreach(string productId in productsIdsList)
            {
                Product product = this.productsRepository.getProductBySiesaId(productId).Result;
                if(product == null)
                    inexistProducts.Add(productId);
            }
            return inexistProducts;
        }

        private List<string> invalidGifts(string giftsConcatSiesaIds)
        {
            List<string> inexistSkusIds = new List<string>();
            List<string> giftConcatSiesaIdsList = JsonSerializer.Deserialize<List<string>>(giftsConcatSiesaIds);
            if(giftConcatSiesaIdsList.Count == 0) return inexistSkusIds;
            foreach (string giftConcatSiesaId in giftConcatSiesaIdsList)
            {
                Sku gift = this.skuRepository.getSkuByConcatSiesaId(giftConcatSiesaId).Result;
                if(gift == null)
                    inexistSkusIds.Add(giftConcatSiesaId);
            }
            return inexistSkusIds;
        }

        private List<string> invalidListSkus(string listConcatSiesaIds)
        {
            List<string> inexistSkusIds = new List<string>();
            List<string> listConcatSiesaIdsList = JsonSerializer.Deserialize<List<string>>(listConcatSiesaIds);
            if(listConcatSiesaIdsList.Count == 0) return inexistSkusIds;
            foreach (string skuConcatSiesaId in listConcatSiesaIdsList)
            {
                Sku sku = this.skuRepository.getSkuByConcatSiesaId(skuConcatSiesaId).Result;
                if (sku == null)
                    inexistSkusIds.Add(skuConcatSiesaId);
            }
            return inexistSkusIds;
        }

        public void Dispose()
        {
            this.loadPromotions.Clear();
            this.inactivatedPromotions.Clear();
            this.failedPromotions.Clear();
            this.notProccecedPromotions.Clear();
            this.inactivePromotions.Clear();
            this.details.Clear();
        }
    }
}
