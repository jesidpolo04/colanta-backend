namespace colanta_backend.App.Brands.Infraestructure
{
    using Brands.Domain;
    public class EFDeltaBrand
    {
        public int id { get; set; }
        public string siesa_id { get; set; }
        public string name { get; set; }

        public Brand getBrandFromEFDeltaBrand()
        {
            return new Brand(this.name, this.siesa_id);
        }

        public void setEFDeltaBrandFromBrand(Brand brand)
        {
            this.siesa_id = brand.id_siesa;
            this.name = brand.name;
        }
    }
}
