namespace colanta_backend.App.Brands.Domain
{
    public class Brand
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string id_siesa { get; set; }
        public int? id_vtex { get; set; } 
        public bool state { get; set; }

        public Brand(string name, string id_siesa, int? id = null, int? id_vtex = null, bool state = true)
        {
            this.id_vtex = id_vtex;
            this.id = id;
            this.id_siesa = id_siesa;
            this.name = name;
            this.state = state;
        }
    }
}
