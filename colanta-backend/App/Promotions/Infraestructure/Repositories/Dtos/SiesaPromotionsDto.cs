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
    }

    public class SiesaPromotionConfiguration
    {
        public decimal? valor { get; set; }
        public decimal? porcentaje { get; set; }
        public int? pague { get; set; }
        public int? lleve { get; set; }
        public string? tipo { get; set; }   
        public ProductoDto[]? items_de_regalo { get; set; }
        public int? cantidad_de_regalos_seleccionables { get; set; }
        public decimal? cantidad_minima_de_items_para_aplicar { get; set; }
        public ProductoDto[]? lista1 { get; set; }
        public ProductoDto[]? lista2 { get; set; }
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

    public class ProductoDto
    {
        public string id_producto { get; set; }
        public string id_variacion { get; set; }
    }

}
