namespace colanta_backend.App.Brands.Infraestructure
{
    using Brands.Domain;
    public class GetVtexBrandDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isActive { get; set; }
        public string? title { get; set; }
        public string? metaTagDescription { get; set; }
        public string? imageUrl { get; set; }

        public Brand toBrand()
        {
            return new Brand(
                    name: this.name,
                    id_vtex: this.id,
                    state: this.isActive
                ); ;
        }

    }
}
