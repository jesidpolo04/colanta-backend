namespace colanta_backend.App.Brands.Domain
{
    public class Brand
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? id_siesa { get; set; }
        public int? id_vtex { get; set; } 
        public string business { get; set; }
        public bool state { get; set; }

        public Brand(string? name, string? id_siesa = null, int? id = null, int? id_vtex = null, string business = "mercolanta", bool state = false)
        {
            this.id_vtex = id_vtex;
            this.id = id;
            this.id_siesa = id_siesa;
            this.name = name;
            this.business = business;
            this.state = state;
        }
    }
}
