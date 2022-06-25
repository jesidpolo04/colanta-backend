namespace colanta_backend.App.Categories.Infraestructure
{
    using Categories.Domain;
    public class SiesaCategoriesDto
    {
        public SiesaCategoryDto[] familias { get; set; }
    }

    public class SiesaCategoryDto
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string negocio { get; set; }
        public SiesaLineDto[] lineas { get; set; }

        public Category toCategory()
        {
            Category father = new Category(
                                    siesa_id: this.id,
                                    name: this.nombre,
                                    business: this.negocio,
                                    isActive: false
                                );
            foreach (SiesaLineDto lineDto in this.lineas)
            {
                Category line = new Category(
                        siesa_id: lineDto.id,
                        name: lineDto.nombre,
                        business: father.business,
                        isActive: false
                    );
                line.setFather(father);
                father.addChild(line);
            }
            return father;
        }
    }

    public class SiesaLineDto
    {
        public string id { get; set; }
        public string nombre { get; set; }
    }
}
