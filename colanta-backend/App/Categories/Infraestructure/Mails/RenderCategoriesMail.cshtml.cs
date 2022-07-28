using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace colanta_backend.App.Categories.Infraestructure
{
    using System.Collections.Generic;
    using Categories.Domain;
    using System;
    public class RenderCategoriesMailModel : PageModel
    {
        public List<Category> loadedCategories;
        public List<Category> inactivatedCategories;
        public List<Category> failedCategories;
        public DateTime dateTime;

        public RenderCategoriesMailModel(List<Category> loadedCategories, List<Category> inactivatedCategories, List<Category> failedCategories)
        {
            this.loadedCategories = loadedCategories;
            this.inactivatedCategories = inactivatedCategories;
            this.failedCategories = failedCategories;
            this.dateTime = DateTime.Now;
        }

        public void OnGet()
        {
        }
    }
}
