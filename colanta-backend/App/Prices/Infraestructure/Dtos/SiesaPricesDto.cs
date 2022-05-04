namespace colanta_backend.App.Prices.Infraestructure
{
    using App.Prices.Domain;
    using App.Products.Domain;
    public class SiesaPricesDto
    {
        public SiesaPriceDto[] precios_productos { get; set; }
    }

    public class SiesaPriceDto
    {
        public string id_producto { get; set; }
        public string? id_variacion { get; set; }
        public decimal precio_publico { get; set; }
        public string negocio { get; set; }

        public Price GetPriceFromDto()
        {
            Price price = new Price();

            price.price = this.precio_publico;
            price.business = this.negocio;
            if(id_variacion != null)
            {
                price.sku_concat_siesa_id = this.negocio + "_" + this.id_producto + "_" + id_variacion;
            }
            else
            {
                price.sku_concat_siesa_id = this.negocio + "_" + this.id_producto + "_" + id_producto;
            }
            return price;
        }
    }
}
