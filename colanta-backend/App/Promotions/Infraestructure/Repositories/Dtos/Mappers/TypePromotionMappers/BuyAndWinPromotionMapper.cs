namespace colanta_backend.App.Promotions.Infraestructure
{
    using Promotions.Domain;
    public class BuyAndWinPromotionMapper : TypePromotionMapper
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
            promotion.type = PromotionTypes.REGALO;
            promotion.gifts_ids = this.mapSkuList(dto.configuracion.items_de_regalo, dto.negocio);
            promotion.list_sku_1_buy_together_ids = this.mapSkuList(dto.configuracion.lista1, dto.negocio);
            promotion.gift_quantity_selectable = (int) dto.configuracion.cantidad_de_regalos_seleccionables;
            promotion.minimum_quantity_buy_together = (int) dto.configuracion.cantidad_minima_de_items_para_aplicar;
            return promotion;
        }
    }
}
