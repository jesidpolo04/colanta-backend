namespace colanta_backend.App.Promotions.Infraestructure
{
    using System;
    using Promotions.Domain;
    using System.Collections.Generic;
    using System.Text.Json;
    public class SiesaPromotionMapper
    {
        public Promotion getPromotionFromDto(SiesaPromotionDto siesaPromotionDto)
        {
            Promotion promotion = new Promotion();
            promotion.siesa_id = siesaPromotionDto.id;
            promotion.business = siesaPromotionDto.negocio;
            promotion.concat_siesa_id = siesaPromotionDto.negocio + "_" + siesaPromotionDto.id;
            
            promotion.name = siesaPromotionDto.nombre;
            promotion.begin_date_utc = siesaPromotionDto.fecha_inicio_utc;
            promotion.end_date_utc = this.setEndDate(siesaPromotionDto.fecha_final_utc);
            promotion.is_active = false;
            promotion.max_number_of_affected_items = siesaPromotionDto.restricciones.maximo_items_validos;
            promotion.max_number_of_affected_items_group_key = this.getRestrictionsPer(siesaPromotionDto.restricciones.maximo_items_validos_por);
            promotion.type = this.getPromotionType(siesaPromotionDto.tipo);

            promotion = this.setConfigurationsProperties(promotion, siesaPromotionDto);
            promotion = this.setApplyProperties(promotion, siesaPromotionDto);

            promotion.cumulative = siesaPromotionDto.restricciones.acumulativa;
            promotion.multiple_use_per_client = siesaPromotionDto.restricciones.uso_multiple;

            return promotion;
        }

        private string getPromotionType(string typeDto)
        {
            string type = "regular";
            switch (typeDto)
            {
                case "porcentual":
                    type = "regular";
                    break;
                case "nominal":
                    type = "regular";
                    break;
                case "bono":
                    type = "forThePriceOf";
                    break;
                case "regalo":
                    type = "buyAndWin";
                    break;
                case "kit":
                    type = "combo";
                    break;
            }
            return type;
        }

        private string getRestrictionsPer(string maxItemValidsPerSiesaDto)
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

        private Promotion setConfigurationsProperties(Promotion promotion, SiesaPromotionDto promotionDto)
        {
            switch (promotionDto.tipo)
            {
                case "porcentual":
                    promotion.discount_type = "percentual";
                    promotion.percentual_discount_value = promotionDto.configuracion.porcentaje != null ? (decimal)promotionDto.configuracion.porcentaje : 0;
                    promotion.gifts_ids = "[]";
                    promotion.list_sku_1_buy_together_ids = "[]";
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;

                case "nominal":
                    promotion.discount_type = "nominal";
                    if (promotionDto.configuracion.valor != null)
                    {
                        promotion.nominal_discount_value = (decimal)promotionDto.configuracion.valor;
                    }
                    promotion.gifts_ids = "[]";
                    promotion.list_sku_1_buy_together_ids = "[]";
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;

                case "bono":
                    promotion.minimum_quantity_buy_together = (int)promotionDto.configuracion.lleve;
                    promotion.quantity_to_affect_buy_together = (int)promotionDto.configuracion.pague;
                    if (promotionDto.configuracion.tipo == "gratis")
                    {
                        promotion.percentual_discount_value = 100;
                    }
                    if (promotionDto.configuracion.tipo == "porcentaje")
                    {
                        promotion.percentual_discount_value = (decimal)promotionDto.configuracion.valor;
                    }
                    if (promotionDto.configuracion.tipo == "maximo_precio")
                    {
                        promotion.maximum_unit_price_discount = (decimal)promotionDto.configuracion.valor;
                    }
                    promotion.gifts_ids = "[]";
                    promotion.list_sku_1_buy_together_ids = "[]";
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;

                case "regalo":
                    List<string> gifts_ids = new List<string>();
                    foreach (ProductoDto gift in promotionDto.configuracion.items_de_regalo)
                    {
                        if(gift.id_variacion == null)
                        {
                            gifts_ids.Add($"{promotion.business}_{gift.id_producto}_{gift.id_producto}");
                        }
                        else
                        {
                            gifts_ids.Add($"{promotion.business}_{gift.id_producto}_{gift.id_variacion}");
                        }
                    }
                    promotion.gifts_ids = JsonSerializer.Serialize(gifts_ids);
                    promotion.gift_quantity_selectable = (int)promotionDto.configuracion.cantidad_de_regalos_seleccionables;
                    promotion.minimum_quantity_buy_together = (int)promotionDto.configuracion.cantidad_minima_de_items_para_aplicar;
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;

                case "kit":

                    List<string> list1 = new List<string>();
                    foreach (ProductoDto product in promotionDto.configuracion.lista1)
                    {
                        if (product.id_variacion == null)
                        {
                            list1.Add($"{promotion.business}_{product.id_producto}_{product.id_producto}");
                        }
                        else
                        {
                            list1.Add($"{promotion.business}_{product.id_producto}_{product.id_variacion}");
                        }
                    }
                    promotion.list_sku_1_buy_together_ids = JsonSerializer.Serialize(list1);

                    List<string> list2 = new List<string>();
                    foreach (ProductoDto product in promotionDto.configuracion.lista2)
                    {
                        if (product.id_variacion == null)
                        {
                            list2.Add($"{promotion.business}_{product.id_producto}_{product.id_producto}");
                        }
                        else
                        {
                            list2.Add($"{promotion.business}_{product.id_producto}_{product.id_variacion}");
                        }
                    }
                    promotion.list_sku_2_buy_together_ids = JsonSerializer.Serialize(list2);

                    promotion.percentual_discount_value_list_1 = (decimal)promotionDto.configuracion.porcentaje_descuento_lista1;
                    promotion.percentual_discount_value_list_2 = (decimal)promotionDto.configuracion.porcentaje_descuento_lista2;
                    promotion.minimum_quantity_buy_together = (int)promotionDto.configuracion.minimo_items_lista_1;
                    promotion.gifts_ids = "[]";
                    break;
                default:
                    break;
            }
            return promotion;
        }

        private Promotion setApplyProperties(Promotion promotion, SiesaPromotionDto promotionDto)
        {
            const string customerTypeField = "customerClass";  

            if (promotionDto.tipo != "bono" && promotionDto.tipo != "regalo")
            {
                List<string> products_ids = new List<string>();
                if (promotionDto.aplica_a.productos != null)
                {
                    foreach (string siesa_id in promotionDto.aplica_a.productos)
                    {
                        products_ids.Add(siesa_id);
                    }
                }
                promotion.products_ids = JsonSerializer.Serialize(products_ids);

                List<string> skus_ids = new List<string>();
                if (promotionDto.aplica_a.variaciones != null)
                {
                    foreach (string siesa_id in promotionDto.aplica_a.variaciones)
                    {
                        skus_ids.Add(siesa_id);
                    }
                }
                promotion.skus_ids = JsonSerializer.Serialize(skus_ids);
            }
            else
            {
                List<string> products_ids = new List<string>();
                if (promotionDto.aplica_a.productos != null)
                {
                    foreach (string siesa_id in promotionDto.aplica_a.productos)
                    {
                        products_ids.Add(siesa_id);
                    }
                }
                promotion.list_sku_1_buy_together_ids = JsonSerializer.Serialize(products_ids);
                promotion.products_ids = "[]";

                List<string> skus_ids = new List<string>();
                if (promotionDto.aplica_a.variaciones != null)
                {
                    foreach (string siesa_id in promotionDto.aplica_a.variaciones)
                    {
                        skus_ids.Add(siesa_id);
                    }
                }
                promotion.list_sku_1_buy_together_ids = JsonSerializer.Serialize(skus_ids);
                promotion.skus_ids = "[]";
            }


            List<string> categories_ids = new List<string>();
            if (promotionDto.aplica_a.categorias != null)
            {
                foreach (string siesa_id in promotionDto.aplica_a.categorias)
                {
                    categories_ids.Add(siesa_id);
                }
            }
            promotion.categories_ids = JsonSerializer.Serialize(categories_ids);

            List<string> brands_ids = new List<string>();
            if (promotionDto.aplica_a.marcas != null)
            {
                foreach (string siesa_id in promotionDto.aplica_a.marcas)
                {
                    brands_ids.Add(siesa_id);
                }
            }
            promotion.brands_ids = JsonSerializer.Serialize(brands_ids);

            List<string> customer_expressions = new List<string>();
            if (promotionDto.aplica_a.tipo_cliente != null)
            {
                foreach(string tipoCliente in promotionDto.aplica_a.tipo_cliente)
                {
                    customer_expressions.Add($"{customerTypeField}=\"{tipoCliente}\"");
                }
            }
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            promotion.cluster_expressions = JsonSerializer.Serialize(customer_expressions, jsonOptions);

            return promotion;
        }

        private string setEndDate(string? UTCDate)
        {
            int defaultMonths = 1;
            if (UTCDate == null) return DateTime.Now.AddMonths(defaultMonths).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            else return UTCDate;
        }
    }
}