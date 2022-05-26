namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;
    using Categories.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System.Text.Json.Serialization;

    public class RenderCategories
    {
        private string processName = "Renderizado de Categorías";
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private CategoriesSiesaRepository siesaRepository;
        private ILogs logs;
        private CustomConsole console;
        private RenderCategoriesMail renderCategoriesMail;

        private List<Category> loadCategories = new List<Category>();
        private List<Category> failedLoadCategories = new List<Category>();
        private List<Category> inactiveCategories = new List<Category>();
        private List<Category> inactivatedCategories = new List<Category>();
        private List<Category> notProccecedCategories = new List<Category>();
        private int obtainedCategories = 0;

        private List<Detail> details = new List<Detail>();
        private JsonSerializerOptions jsonOptions;
        public RenderCategories(
            CategoriesRepository categoriesLocalRepository, 
            CategoriesVtexRepository categoriesVtexRepository, 
            CategoriesSiesaRepository categoriesSiesaRepository,
            ILogs logs,
            EmailSender emailSender
        )
        {
            this.localRepository = categoriesLocalRepository;
            this.vtexRepository = categoriesVtexRepository;
            this.siesaRepository = categoriesSiesaRepository;
            this.logs = logs;
            this.console = new CustomConsole();
            this.renderCategoriesMail = new RenderCategoriesMail(emailSender);

            this.jsonOptions = new JsonSerializerOptions();
            this.jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            this.jsonOptions.ReferenceHandler = ReferenceHandler.Preserve;

            
        }

        public async Task<bool> Invoke()
        {
            this.console.warningColor().write("Iniciando proceso:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

            try
            {
                Category[] siesaCategories = await this.siesaRepository.getAllCategories();
                this.details.Add(new Detail(
                                    origin: "siesa",
                                    action: "traer todas las categorías",
                                    description: "se obtuvieron con éxito todas las categorías",
                                    success: true,
                                    content: JsonSerializer.Serialize(siesaCategories, jsonOptions)
                                ));
                //inactive delta categories

                Category[] deltaCategories = await this.localRepository.getDeltaCategories(siesaCategories);
                if (deltaCategories.Length > 0)
                {
                    foreach (Category category in deltaCategories)
                    {
                        try
                        {
                            category.isActive = false;
                            await this.vtexRepository.updateCategory(category);
                            this.inactivatedCategories.Add(category);
                            this.details.Add(new Detail(
                                    origin: "vtex", 
                                    action: "actualizar estado de la categoría", 
                                    description: "se actualizó con éxito el estado de la categoría", 
                                    success: true, 
                                    content: JsonSerializer.Serialize(category, jsonOptions)
                                ));
                        }
                        catch (VtexException vtexException)
                        {
                            category.isActive = true;
                            this.failedLoadCategories.Add(category);
                            this.details.Add(new Detail(
                                origin: "vtex",
                                action: "actualizar estado de la categoría", 
                                description: vtexException.Message,
                                content: vtexException.Message,
                                success: false));
                        }
                    }
                    await this.localRepository.updateCategories(deltaCategories);
                }

                foreach (Category siesaCategory in siesaCategories)
                {
                    this.obtainedCategories++;
                    foreach(Category siesaChildCategory in siesaCategory.childs)
                    {
                        this.obtainedCategories++;
                    }

                    Category? localCategory = await this.localRepository.getCategoryBySiesaId(siesaCategory.siesa_id);

                    if (localCategory != null)
                    {
                        if (localCategory.isActive)
                        {
                            this.notProccecedCategories.Add(localCategory);
                        }

                        if (!localCategory.isActive)
                        {
                            this.inactiveCategories.Add(localCategory);
                        }
                    }
                    else
                    {
                        try
                        {
                            localCategory = await this.localRepository.saveCategory(siesaCategory);
                            Category vtexCategory = await this.vtexRepository.saveCategory(localCategory);
                            localCategory.vtex_id = vtexCategory.vtex_id;
                            localCategory = await this.localRepository.updateCategory(localCategory);
                            this.loadCategories.Add(localCategory);
                            this.details.Add(new Detail(
                                    origin: "vtex",
                                    action: "cargar categoría",
                                    description: "se cargó correctamente la categoría",
                                    success: true,
                                    content: JsonSerializer.Serialize(localCategory, jsonOptions)
                                ));
                            foreach (Category localCategoryChild in localCategory.childs)
                            {
                                Category vtexCategoryChild = await this.vtexRepository.saveCategory(localCategoryChild);
                                localCategoryChild.vtex_id = vtexCategoryChild.vtex_id;
                                await this.localRepository.updateCategory(localCategoryChild);
                                this.loadCategories.Add(localCategoryChild);
                                this.details.Add(new Detail(
                                        origin: "vtex",
                                        action: "cargar categoría",
                                        description: "se cargó correctamente la categoría",
                                        success: true,
                                        content: JsonSerializer.Serialize(localCategoryChild, jsonOptions)
                                    ));
                            }
                        }
                        catch (VtexException exception)
                        {
                            this.failedLoadCategories.Add(siesaCategory);
                            this.details.Add(new Detail(
                                   origin: "vtex",
                                   action: "cargar categoría",
                                   description: exception.Message,
                                   success: false,
                                   content: exception.Message
                               ));
                        }
                    }

                }
                this.writeConsoleLogs();
                this.logs.Log(
                        this.processName,
                        this.loadCategories.Count,
                        this.failedLoadCategories.Count,
                        this.notProccecedCategories.Count,
                        this.obtainedCategories,
                        JsonSerializer.Serialize(this.details, jsonOptions)
                    );
                this.renderCategoriesMail.sendMail(
                        this.loadCategories.ToArray(),
                        this.inactiveCategories.ToArray(),
                        this.failedLoadCategories.ToArray(),
                        this.notProccecedCategories.ToArray(),
                        this.inactivatedCategories.ToArray()
                    );

                this.console.warningColor().write("Proceso Finalizado:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

                return true;
            }
            catch (SiesaException vtexException)
            {
                this.details.Add(new Detail(
                                   origin: "vtex",
                                   action: "cargar categoría",
                                   description: vtexException.Message,
                                   success: false,
                                   content: vtexException.Message
                               ));
                this.logs.Log(
                        name: this.processName,
                        total_errors: 1,
                        total_loads: 0,
                        total_not_procecced: 0,
                        total_obtained: 0,
                        json_details: JsonSerializer.Serialize(this.details, jsonOptions)
                    );

                this.console.warningColor().write("Proceso Finalizado:")
                .infoColor().write(this.processName)
                .grayColor().write("Fecha:")
                .magentaColor().write(DateTime.Now.ToString()).endPharagraph();

                return false;
            }
        }

        private void writeConsoleLogs()
        {
            if (this.inactivatedCategories.Count > 0)
            {
                this.console.errorColor().writeLine("Categorías desactivadas");
                foreach (Category inactivatedCategory in this.inactivatedCategories)
                {
                    this.console.whiteColor().write(inactivatedCategory.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(inactivatedCategory.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(inactivatedCategory.siesa_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.loadCategories.Count > 0)
            {
                this.console.successColor().writeLine("Categorías Creadas");
                foreach (Category loadCategory in this.loadCategories)
                {
                    this.console.whiteColor().write(loadCategory.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(loadCategory.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(loadCategory.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.failedLoadCategories.Count > 0)
            {
                this.console.errorColor().writeLine("Categorías no subidas a VTEX debido a error");
                foreach (Category failedCategory in this.failedLoadCategories)
                {
                    this.console.whiteColor().write(failedCategory.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(failedCategory.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(failedCategory.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.inactiveCategories.Count > 0)
            {
                this.console.warningColor().writeLine("Categorías por activar en Vtex");
                foreach (Category inactiveCategory in this.inactiveCategories)
                {
                    this.console.whiteColor().write(inactiveCategory.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(inactiveCategory.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(inactiveCategory.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }

            if (this.notProccecedCategories.Count > 0)
            {
                this.console.successColor().writeLine("Categorías no procesadas");
                foreach (Category notProccecedCategory in this.notProccecedCategories)
                {
                    this.console.whiteColor().write(notProccecedCategory.name)
                        .grayColor().write("siesa id: ")
                        .infoColor().write(notProccecedCategory.siesa_id)
                        .grayColor().write("vtex id:")
                        .infoColor().write(notProccecedCategory.vtex_id.ToString()).skipLine();
                }
                this.console.endPharagraph();
            }
        }
    }
}
