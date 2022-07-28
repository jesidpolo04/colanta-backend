namespace colanta_backend.App.Categories.Infraestructure
{
    using Categories.Domain;
    public class VtexCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? FatherCategoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public bool IsActive { get; set; }
        public string? LomadeeCampaignCode { get; set; }
        public string? AdWordsRemarketingCode { get; set; }
        public bool ShowInStoreFront { get; set; }
        public bool ShowBrandFilter { get; set; }
        public bool ActiveStoreFrontLink { get; set; }
        public int? GlobalCategoryId { get; set; }
        public string? StockKeepingUnitSelectionMode { get; set; }
        public int? Score { get; set; }
        public string? LinkId { get; set; }
        public bool HasChildren { get; set; }

        public Category toCategory()
        {
            var category = new Category(
                            name: this.Name,
                            vtex_id: this.Id,
                            isActive: this.IsActive
                        );
            category.setFather(new Category(vtex_id: FatherCategoryId, name: ""));
            return category;
        }
    }
}
