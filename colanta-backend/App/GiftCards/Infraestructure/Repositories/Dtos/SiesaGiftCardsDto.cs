namespace colanta_backend.App.GiftCards.Infraestructure
{
    using GiftCards.Domain;
    public class SiesaGiftCardsDto
    {
        public SiesaGiftCardDto[] tarjetas { get; set; }
    }

    public class SiesaGiftCardDto
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public decimal balance { get; set; }
        public string codigo { get; set; }
        public string propietario { get; set; }
        public string fecha_expiracion { get; set; }
        public string fecha_emision { get; set; }
        public string negocio { get; set; }

        public GiftCard getGiftCardFromDto()
        {
            GiftCard giftCard = new GiftCard();
            giftCard.siesa_id = this.id;
            giftCard.name = this.nombre;
            giftCard.balance = this.balance;
            giftCard.code = this.codigo;
            giftCard.token = this.codigo;
            giftCard.owner = this.propietario;
            giftCard.expire_date = this.fecha_expiracion;
            giftCard.emision_date = this.fecha_emision;
            giftCard.business = this.negocio;
            return giftCard;
        }

    }
}
