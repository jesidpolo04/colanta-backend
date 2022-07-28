namespace colanta_backend.App.Categories.Jobs
{
    using App.Categories.Domain;
    using App.Shared.Domain;
    using App.Shared.Application;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public class UpCategoriesToVtex
    {
        private string processName = "Carga de categorías nulas a Vtex";
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private IProcess logs;
        private List<Detail> details;
        private JsonSerializerOptions jsonOptions;

        private List<Category> failedCategories;
        private List<Category> loadCategories;

        public UpCategoriesToVtex(CategoriesRepository localRepository, CategoriesVtexRepository vtexRepository, IProcess logs)
        {
            
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logs = logs;
            this.details = new List<Detail>();
            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            this.jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

            this.failedCategories = new List<Category>();
            this.loadCategories = new List<Category>();
        }

        public async Task Invoke()
        {
            Category[] nullVtexCategories = await this.localRepository.getVtexNullCategories();

            foreach(Category nullVtexCategory in nullVtexCategories)
            {
                try
                {
                    Category vtexCategory = await this.vtexRepository.saveCategory(nullVtexCategory);
                    nullVtexCategory.vtex_id = vtexCategory.vtex_id;
                    await this.localRepository.updateCategory(nullVtexCategory);
                    this.loadCategories.Add(nullVtexCategory);
                    this.details.Add(new Detail(
                            origin: "vtex",
                            action: "crear categoría",
                            content: JsonSerializer.Serialize(nullVtexCategory, this.jsonOptions),
                            description: "se creó correctamente la categoría",
                            success: true
                        ));
                }
                catch (VtexException exception)
                {
                    this.failedCategories.Add(nullVtexCategory);
                    this.details.Add(new Detail(
                            origin: "vtex",
                            action: "crear categoría",
                            content: exception.Message,
                            description: exception.Message,
                            success: false
                        ));
                }
            }
            this.logs.Log(
                       name: processName,
                       total_loads: this.loadCategories.Count,
                       total_errors: this.failedCategories.Count,
                       total_not_procecced: 0,
                       total_obtained: 0,
                       JsonSerializer.Serialize(this.details, this.jsonOptions)
                   );
        }
    }
}
