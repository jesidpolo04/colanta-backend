namespace colanta_backend.App.Categories.Infraestructure
{
    using System.Text.Json.Serialization;
    using Categories.Domain;
    
    public class SiesaCategoriesDto
    {
        [JsonPropertyName("familias")]
        public SiesaCategoryDto[] Familias { get; set; }
    }

    public class SiesaCategoryDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
        
        [JsonPropertyName("negocio")]
        public string Negocio { get; set; }
        
        [JsonPropertyName("lineas")]
        public SiesaLineDto[] Lineas { get; set; }

        public Category ToCategory()
        {
            Category father = new Category(
                                    siesa_id: Id,
                                    name: Nombre,
                                    business: Negocio,
                                    isActive: false
                                );
            foreach (SiesaLineDto lineDto in this.Lineas)
            {
                Category line = new Category(
                        siesa_id: lineDto.Id,
                        name: lineDto.Nombre,
                        business: father.Business,
                        isActive: false
                    );
                line.SetFather(father);
                father.AddChild(line);
            }
            return father;
        }
    }

    public class SiesaLineDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}
