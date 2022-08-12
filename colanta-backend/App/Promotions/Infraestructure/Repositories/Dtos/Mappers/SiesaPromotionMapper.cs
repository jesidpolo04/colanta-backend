namespace colanta_backend.App.Promotions.Infraestructure
{
    using System;
    using Promotions.Domain;
    using System.Collections.Generic;
    using System.Text.Json;
    public class SiesaPromotionMapper
    {
        private TypePromotionMapper mapper;

        public Promotion getPromotionFromDto(SiesaPromotionDto dto)
        {
            if (dto.tipo == "porcentual") mapper = new PercentualPromotionMapper();
            else if (dto.tipo == "nominal") mapper = new NominalPromotionMapper();
            else if (dto.tipo == "bono") mapper = new ForThePriceOfPromotionMapper();
            else if (dto.tipo == "regalo") mapper = new BuyAndWinPromotionMapper();
            else if (dto.tipo == "kit") mapper = new ComboPromotionMapper();
            else throw new Exception($"{dto.tipo} no es un tipo de promoción válida");
            return mapper.Map(dto);
        }
    }
}