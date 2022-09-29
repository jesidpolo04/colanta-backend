namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    public abstract class TypePromotionMapper
    {
        protected JsonSerializerOptions jsonOptions;
        protected string customerTypeField = "customerClass";
        
        public TypePromotionMapper()
        {
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }

        public abstract Promotion Map(SiesaPromotionDto promotionDto);

        protected string setEndDate(string? UTCDate)
        {
            int defaultMonths = 1;
            int daysToAdd = 1;
            if (UTCDate == null) return DateTime.Now.AddMonths(defaultMonths).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            else return DateTime.Parse(UTCDate).AddDays(daysToAdd).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        protected string getRestrictionsPer(string maxItemValidsPerSiesaDto)
        {
            string maxItemsValidPer;
            switch (maxItemValidsPerSiesaDto)
            {
                case "carrito":
                    maxItemsValidPer = "perCart";
                    break;
                case "producto":
                    maxItemsValidPer = "perProduct";
                    break;
                case "variacion":
                    maxItemsValidPer = "perSku";
                    break;
                default:
                    maxItemsValidPer = "perCart";
                    break;
            }
            return maxItemsValidPer;
        }

        protected Promotion setValidApplications(Promotion promotion, SiesaPromotionDto dto)
        {
            promotion.brands_ids = dto.aplica_a.marcas != null ?
                this.mapApplyBrands(dto.aplica_a.marcas) : "[]";

            promotion.categories_ids = dto.aplica_a.categorias != null ?
                this.mapApplyCategories(dto.aplica_a.categorias) : "[]";
        
            promotion.products_ids = dto.aplica_a.productos != null ?
                this.mapApplyProducts(dto.aplica_a.productos) : "[]";

            promotion.skus_ids = dto.aplica_a.variaciones != null ?
                this.mapApplyVariations(dto.aplica_a.variaciones) : "[]";

            promotion.cluster_expressions = dto.aplica_a.tipo_cliente != null ?
                this.mapCustomerClusters(dto.aplica_a.tipo_cliente) : "[]";

            return promotion;
        }

        protected string mapApplyBrands(string[] brandsIds)
        {
            return JsonSerializer.Serialize(brandsIds);
        }

        protected string mapApplyCategories(string[] categoriesIds)
        {
            return JsonSerializer.Serialize(categoriesIds);
        }

        protected string mapApplyProducts(string[] productsIds)
        {
            return JsonSerializer.Serialize(productsIds);
        }

        protected string mapApplyVariations(string[] variationsIds)
        {
            return JsonSerializer.Serialize(variationsIds);
        }

        protected string mapSkuList(ProductoDto[] products, string business)
        {
            List<string> skuConcatSiesaIds = new List<string>();
            foreach(ProductoDto product in products)
            {
                if (product.id_variacion != null) skuConcatSiesaIds.Add($"{business}_{product.id_producto}_{product.id_variacion}");
                else skuConcatSiesaIds.Add($"{business}_{product.id_producto}_{product.id_producto}");
            }
            return JsonSerializer.Serialize(skuConcatSiesaIds);
        }

        protected string mapCustomerClusters(string[] customersIds)
        {
            List<string> customer_expressions = new List<string>();

                foreach (string customerId in customersIds)
                {
                    customer_expressions.Add($"{customerTypeField}=\"{customerId}\"");
                }
            
            return JsonSerializer.Serialize(customer_expressions, jsonOptions);
        }
    }
}
