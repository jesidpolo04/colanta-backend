using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Brands.Infraestructure
{
    using Brands.Domain;
    using System.Collections.Generic;
    using System;
    public class RenderBrandsMailModel : PageModel
    {
        public List<Brand> loadedBrands;
        public List<Brand> inactivatedBrands;
        public List<Brand> failedBrands;
        public DateTime dateTime;
        public RenderBrandsMailModel(List<Brand> loadedBrands, List<Brand> inactivatedBrands, List<Brand> failedBrands)
        {
            this.loadedBrands = loadedBrands;
            this.inactivatedBrands = inactivatedBrands;
            this.failedBrands = failedBrands;
            this.dateTime = DateTime.Now;
        }

        public void OnGet(List<Brand> loadedBrands, List<Brand> inactivatedBrands, List<Brand> failedBrands)
        {
            this.loadedBrands = loadedBrands;
            this.inactivatedBrands = inactivatedBrands;
            this.failedBrands = failedBrands;
        }
    }
}
