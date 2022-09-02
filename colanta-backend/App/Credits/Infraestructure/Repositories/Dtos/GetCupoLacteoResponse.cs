namespace colanta_backend.App.Credits.Infraestructure
{
    using System;
    using Credits.Domain;
    using Shared.Domain;
    using GiftCards.Domain;
    public class GetCupoLacteoResponse
    {
        public decimal cupo { get; set; }
        public string codigo { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime fecha_expiracion { get; set; }

        public GiftCard getGiftcard()
        {
            GiftCard card = new GiftCard();
            card.siesa_id = this.codigo;
            card.code = this.codigo;
            card.token = this.codigo;
            card.name = "Cupo Lácteo";
            card.provider = Providers.CUPO;
            card.emision_date = this.fecha_creacion.ToString(DateFormats.UTC);
            card.expire_date = this.fecha_expiracion.ToString(DateFormats.UTC);
            card.balance = this.cupo;
            card.used = false;
            return card;
        }
    }
}
