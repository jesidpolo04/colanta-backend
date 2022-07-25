namespace colanta_backend.App.Products.Infraestructure
{
    using Products.Domain;
    public class ByRefIdVtexProductDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string? LinkId { get; set; }
        public string? RefId { get; set; }
        public bool? IsVisible { get; set; }
        public string? Description { get; set; }
        public string? DescriptionShort { get; set; }
        public string? ReleaseDate { get; set; }
        public string? KeyWords { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; }
        public string? TaxCode { get; set; }
        public string? MetaTagDescription { get; set; }
        public int? SupplierId { get; set; }
        public bool ShowWithoutStock { get; set; }
        public int[]? ListStoreId { get; set; }
        public string? AdWordsRemarketingCode { get; set; }
        public string? LomadeeCampaignCode { get; set; }
        public int? Score { get; set; }

        public Product getProductFromDto()
        {
            Product product = new Product();

            product.vtex_id = this.Id;
            product.name = this.Name;
            product.description = this.Description;
            product.concat_siesa_id = this.RefId;
            product.is_active = this.IsActive;

            return product;
        }
    }
}
