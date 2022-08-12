namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    public class ComboPromotionMapper : TypePromotionMapper
    {
        public new Promotion Map(SiesaPromotionDto promotionDto)
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
            promotion.type = PromotionTypes.KIT;
            promotion.list_sku_1_buy_together_ids = this.mapSkuList(dto.configuracion.lista1, dto.negocio);
            promotion.list_sku_2_buy_together_ids = this.mapSkuList(dto.configuracion.lista2, dto.negocio);
            promotion.percentual_discount_value_list_1 = (decimal) dto.configuracion.porcentaje_descuento_lista1;
            promotion.percentual_discount_value_list_2 = (decimal) dto.configuracion.porcentaje_descuento_lista2;
            promotion.minimum_quantity_buy_together = (int) dto.configuracion.minimo_items_lista_1;
            return promotion;
        }
    }
}
