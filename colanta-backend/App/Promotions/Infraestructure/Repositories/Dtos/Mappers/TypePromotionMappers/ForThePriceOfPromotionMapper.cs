namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    using System;
    using System.Text.Json;
    public class ForThePriceOfPromotionMapper : TypePromotionMapper
    {
        public override Promotion Map(SiesaPromotionDto promotionDto)
        {
            Promotion promotion = new Promotion();

            promotion.siesa_id = promotionDto.id;
            promotion.business = promotionDto.negocio;
            promotion.concat_siesa_id = $"{promotion.business}_{promotion.id}";
            promotion.name = promotionDto.nombre;
            promotion.begin_date_utc = promotionDto.fecha_inicio_utc;
            promotion.end_date_utc = this.setEndDate(promotionDto.fecha_final_utc);
            promotion.is_active = false;
            promotion.max_number_of_affected_items = promotionDto.restricciones.maximo_items_validos;
            promotion.max_number_of_affected_items_group_key = this.getRestrictionsPer(promotionDto.restricciones.maximo_items_validos_por);
            promotion.cumulative = promotionDto.restricciones.acumulativa;
            promotion.multiple_use_per_client = promotionDto.restricciones.uso_multiple;
            promotion = this.setConfiguration(promotion, promotionDto);
            promotion = this.setValidApplications(promotion, promotionDto);
            return promotion;
        }

        private Promotion setConfiguration(Promotion promotion, SiesaPromotionDto dto)
        {
            promotion.type = PromotionTypes.BONO;
            promotion.minimum_quantity_buy_together = (int)dto.configuracion.lleve;
            promotion.quantity_to_affect_buy_together = (int)dto.configuracion.pague;
            promotion = this.setDiscountType(promotion, dto);
            promotion.gifts_ids = "[]";
            promotion.list_sku_1_buy_together_ids = this.mapSkuList(dto.configuracion.lista1, dto.negocio);
            promotion.list_sku_2_buy_together_ids = "[]";
            return promotion;
        }

        private Promotion setDiscountType(Promotion promotion, SiesaPromotionDto dto)
        {
            if (dto.configuracion.tipo == "gratis")
            {
                promotion.percentual_discount_value = 100;
            }
            if (dto.configuracion.tipo == "porcentaje")
            {
                promotion.percentual_discount_value = (decimal)dto.configuracion.valor;
            }
            if (dto.configuracion.tipo == "maximo_precio")
            {
                promotion.maximum_unit_price_discount = (decimal)dto.configuracion.valor;
            }
            return promotion;
        }

        
    }
}