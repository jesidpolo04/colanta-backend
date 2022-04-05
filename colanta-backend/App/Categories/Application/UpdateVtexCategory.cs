namespace colanta_backend.App.Categories.Application
{
    using System.Threading.Tasks;
    using Shared.Domain;
    using Categories.Domain;
    using System;
    public class UpdateVtexCategory
    {
        private CategoriesVtexRepository categoriesVtexRepository;
        private int _numberOfTry = 5;

        public UpdateVtexCategory(CategoriesVtexRepository categoriesVtexRepository)
        {
            this.categoriesVtexRepository = categoriesVtexRepository;
        }

        public async Task<Category> Invoke(Category category)
        {
            try
            {
                return await this.categoriesVtexRepository.updateCategory(category);
            }
            catch(Exception exception)
            {
                for (int i = 1; i <= this._numberOfTry; i++)
                {
                    //this.console.color(ConsoleColor.Yellow).write("Reintentando inserción, intento:")
                    //    .color(ConsoleColor.White).write("" + (i)).skipLine();
                    try
                    {
                        return await this.categoriesVtexRepository.updateCategory(category);
                        //this.console.color(ConsoleColor.DarkGreen).write("Inserción a VTEX exitosa:")
                        //    .color(ConsoleColor.White).write("" + (i)).dot(2);
                    }
                    catch
                    {
                        if (i == this._numberOfTry)
                        {
                            throw exception;
                        }
                    }
                }
                return null;
            }
        }
    }
}
