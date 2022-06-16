namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    using System.Text.Json;
    using System.Collections.Generic;
    public class SiesaPromotionsDto
    {
        public SiesaPromotionDto[] promociones { get; set; }
    }

    public class SiesaPromotionDto
    {
        public string id { get; set; }
        public string negocio { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
        public string fecha_inicio_utc { get; set; }
        public string fecha_final_utc { get; set; }
        public SiesaPromotionConfiguration configuracion { get; set; }
        public SiesaPromotionAplicaA aplica_a { get; set; }
        public SiesaPromotionRestricciones restricciones { get; set; }

        public Promotion getPromotionFromDto()
        {
            Promotion promotion = new Promotion();
            promotion.siesa_id = this.id;
            promotion.business = this.negocio;
            promotion.concat_siesa_id = this.negocio + "_" + this.id;
            switch (this.tipo)
            {
                case "porcentual":
                    promotion.type = "regular";
                    promotion.discount_type = "percentual";
                    if(this.configuracion.porcentaje != null)
                    {
                        promotion.percentual_discount_value = (decimal)this.configuracion.porcentaje;
                    }
                    promotion.gifts_ids = "[]";
                    promotion.list_sku_1_buy_together_ids = "[]";
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;
                case "nominal":
                    promotion.type = "regular";
                    promotion.discount_type = "nominal";
                    if (this.configuracion.valor != null)
                    {
                        promotion.nominal_discount_value = (decimal)this.configuracion.valor;
                    }
                    promotion.gifts_ids = "[]";
                    promotion.list_sku_1_buy_together_ids = "[]";
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;
                case "bono":
                    promotion.type = "forThePriceOf";
                    promotion.minimum_quantity_buy_together = (int) this.configuracion.lleve;
                    promotion.quantity_to_affect_buy_together = (int) this.configuracion.pague;
                    if (this.configuracion.tipo == "gratis")
                    {
                        promotion.percentual_discount_value = 100;
                    }
                    if(this.configuracion.tipo == "porcentaje")
                    {
                        promotion.percentual_discount_value = (decimal) this.configuracion.valor;
                    }
                    if(this.configuracion.tipo == "maximo_precio")
                    {
                        // implementar cambiar entidad
                    }
                    promotion.gifts_ids = "[]";
                    promotion.list_sku_1_buy_together_ids = "[]";
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;
                case "regalo":
                    promotion.type = "buyAndWin";
                    List<string> gifts_ids = new List<string>();
                    foreach(string gift_id in this.configuracion.items_de_regalo)
                    {
                        gifts_ids.Add(gift_id);
                    }
                    promotion.gifts_ids = JsonSerializer.Serialize(gifts_ids);
                    promotion.gift_quantity_selectable = (int) this.configuracion.cantidad_de_regalos_seleccionables;
                    promotion.minimum_quantity_buy_together = (int) this.configuracion.cantidad_minima_de_items_para_aplicar;
                    promotion.list_sku_2_buy_together_ids = "[]";
                    break;
                case "kit":
                    promotion.type = "combo";

                    List<string> list1 = new List<string>();
                    foreach(string sku_siesa_id in this.configuracion.lista1)
                    {
                        list1.Add(sku_siesa_id);
                    }
                    promotion.list_sku_1_buy_together_ids = JsonSerializer.Serialize(list1);

                    List<string> list2 = new List<string>();
                    foreach (string sku_siesa_id in this.configuracion.lista1)
                    {
                        list2.Add(sku_siesa_id);
                    }
                    promotion.list_sku_2_buy_together_ids = JsonSerializer.Serialize(list2);

                    promotion.percentual_discount_value_list_1 = (decimal) this.configuracion.porcentaje_descuento_lista1;
                    promotion.percentual_discount_value_list_2 = (decimal) this.configuracion.porcentaje_descuento_lista2;
                    promotion.minimum_quantity_buy_together = (int) this.configuracion.minimo_items_lista_1;
                    promotion.gifts_ids = "[]";
                    break;
                default:
                    break;
            }
            promotion.name = this.nombre;
            promotion.begin_date_utc = this.fecha_inicio_utc;
            promotion.end_date_utc = this.fecha_final_utc;
            promotion.is_active = false;
            promotion.max_number_of_affected_items = this.restricciones.maximo_items_validos;

            switch (this.restricciones.maximo_items_validos_por)
            {
                case "carrito":
                    promotion.max_number_of_affected_items_group_key = "perCart";
                    break;
                case "producto":
                    promotion.max_number_of_affected_items_group_key = "perProduct";
                    break;
                case "variacion":
                    promotion.max_number_of_affected_items_group_key = "perSku";
                    break;
                default:
                    promotion.max_number_of_affected_items_group_key = "perCart";
                    break;
            }

            List<string> products_ids = new List<string>();
            foreach(string siesa_id in this.aplica_a.productos)
            {
                products_ids.Add(siesa_id);
            }
            promotion.products_ids = JsonSerializer.Serialize(products_ids);

            if (this.tipo != "bono" && this.tipo != "regalo")
            {
                List<string> skus_ids = new List<string>();
                foreach (string siesa_id in this.aplica_a.variaciones)
                {
                    skus_ids.Add(siesa_id);
                }
                promotion.skus_ids = JsonSerializer.Serialize(skus_ids);
            }
            else
            {
                List<string> skus_ids = new List<string>();
                foreach (string siesa_id in this.aplica_a.variaciones)
                {
                    skus_ids.Add(siesa_id);
                }
                promotion.list_sku_1_buy_together_ids = JsonSerializer.Serialize(skus_ids);
                promotion.skus_ids = "[]";
            }
           

            List<string> categories_ids = new List<string>();
            foreach (string siesa_id in this.aplica_a.categorias)
            {
                categories_ids.Add(siesa_id);
            }
            promotion.categories_ids = JsonSerializer.Serialize(categories_ids);

            List<string> brands_ids = new List<string>();
            foreach (string siesa_id in this.aplica_a.marcas)
            {
                brands_ids.Add(siesa_id);
            }
            promotion.brands_ids = JsonSerializer.Serialize(brands_ids);

            promotion.cumulative = this.restricciones.acumulativa;
            promotion.multiple_use_per_client = this.restricciones.uso_multiple;

            return promotion;
        }
    }

    public class SiesaPromotionConfiguration
    {
        public decimal? valor { get; set; }
        public decimal? porcentaje { get; set; }
        public int? pague { get; set; }
        public int? lleve { get; set; }
        public string? tipo { get; set; }   
        public string[]? items_de_regalo { get; set; }
        public int? cantidad_de_regalos_seleccionables { get; set; }
        public int? cantidad_minima_de_items_para_aplicar { get; set; }
        public string[]? lista1 { get; set; }
        public string[]? lista2 { get; set; }
        public decimal? porcentaje_descuento_lista1 { get; set; }
        public decimal? porcentaje_descuento_lista2 { get; set; }
        public int? minimo_items_lista_1 { get; set; }

    }

    public class SiesaPromotionAplicaA
    {
        public string[] marcas { get; set; }
        public string[] categorias { get; set; }
        public string[] productos { get; set; }
        public string[] variaciones { get; set; }
        public string[] tipo_cliente { get; set; }
    }

    public class SiesaPromotionRestricciones
    {
        public bool acumulativa { get; set; }
        public bool uso_multiple { get; set; }
        public decimal valor_minimo_compra { get; set; }
        public decimal valor_maximo_compra { get; set; }
        public string maximo_items_validos_por { get; set; }
        public int maximo_items_validos { get; set; }
    }

}
