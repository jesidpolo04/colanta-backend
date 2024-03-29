﻿using colanta_backend.App.Promotions.Domain;

namespace colanta_backend.App.Promotions.Infraestructure
{
    using System;
    using System.Text.Json;
    public class PercentualPromotionMapper : TypePromotionMapper
    {
        public override Promotion Map(SiesaPromotionDto promotionDto)
        {
            Promotion promotion = new Promotion();

            promotion.siesa_id = promotionDto.id;
            promotion.business = promotionDto.negocio;
            promotion.concat_siesa_id = $"{promotion.business}_{promotion.id}";
            promotion.name = getPromotionName(promotionDto.id, promotionDto.nombre);
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
            promotion.type = PromotionTypes.PORCENTUAL;
            promotion.discount_type = PromotionDiscountTypes.PORCENTUAL;
            promotion.price_table_name = $"promocional_{promotion.siesa_id}";
            promotion.percentual_discount_value = (decimal) dto.configuracion.porcentaje;
            promotion.gifts_ids = "[]";
            promotion.list_sku_1_buy_together_ids = "[]";
            promotion.list_sku_2_buy_together_ids = "[]";
            return promotion;
        }

        private string getPromotionName(string promotionSiesaId, string promotionName){
            return $"{promotionSiesaId}_{promotionName}";
        }
    }
}
