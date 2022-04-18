namespace colanta_backend.App.Categories.Infraestructure
{
    using Categories.Domain;
    public class VtexTreeCategoryDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool hasChildren { get; set; }
        public string url { get; set; }
        public VtexTreeCategoryDto[] children { get; set; }
        public string? Title { get; set; }
        public string? MetaTagDescription { get; set; }

        public Category toCategory()
        {
            Category family = new Category(
                                        name: this.name,
                                        vtex_id: this.id
                                    );
            foreach(VtexTreeCategoryDto lineDto in this.children)
            {
                Category line = new Category(
                                        name: lineDto.name,
                                        vtex_id: lineDto.id
                                    );
                line.setFather(family);
                family.addChild(line);
            }
            return family;
        }
    }

}
