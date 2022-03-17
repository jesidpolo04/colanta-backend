using System;
using System.Collections.Generic;

#nullable disable

namespace colanta_backend.App.Brands.Infraestructure
{
    using App.Brands.Domain;
    public partial class EFBrand
    {
        public int? id { get; set; }
        public int? id_vtex { get; set; }
        public string id_siesa { get; set; }
        public string name { get; set; }
        public short? state { get; set; }

        public Brand getBrandFromEFBrand()
        {
            return new Brand(
                id: this.id,
                id_vtex: this.id_vtex,
                id_siesa: this.id_siesa,
                name: this.name,
                state: Convert.ToBoolean(this.state)
            );
        }

        public void setEFBrandFromBrand(Brand brand)
        {
            this.id = brand.id;
            this.id_vtex = brand.id_vtex;
            this.id_siesa = brand.id_siesa;
            this.name = brand.name;
            this.state = Convert.ToInt16(brand.state);
        }
    }
    
   
}
