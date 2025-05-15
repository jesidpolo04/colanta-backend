namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Text.Json;
    using Categories.Domain;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;

    public class RenderCategories : IDisposable
    {
        private bool _disposed = false;
        private readonly ICategoriesRepository _localRepository;
        private readonly ICategoriesVtexRepository _vtexRepository;
        private readonly ICategoriesSiesaRepository _siesaRepository;
        private readonly ILogger<RenderCategories> _logger;
        private readonly IRenderCategoriesMail _mail;

        private readonly List<Category> _loadCategories = new List<Category>();
        private readonly List<Category> _failedLoadCategories = new List<Category>();
        private readonly List<Category> _inactiveCategories = new List<Category>();
        private readonly List<Category> _inactivatedCategories = new List<Category>();
        private readonly List<Category> _notProccecedCategories = new List<Category>();

        public RenderCategories(
            ICategoriesRepository categoriesLocalRepository, 
            ICategoriesVtexRepository categoriesVtexRepository, 
            ICategoriesSiesaRepository categoriesSiesaRepository,
            ILogger<RenderCategories> logger,
            IRenderCategoriesMail mail
        )
        {
            _localRepository = categoriesLocalRepository;
            _vtexRepository = categoriesVtexRepository;
            _siesaRepository = categoriesSiesaRepository;
            this._mail = mail;
            _logger = logger;
        }

        /// <summary>
        /// Renderiza las categorías de SIESA en VTEX, desactivando las que ya no vienen en el JSON proporcionado por SIESA y creando las nuevas
        /// </summary>
        public async Task Invoke()
        {
            try
            {
                _logger.LogTrace("Iniciando renderizado de categorías, fecha: {Date}", DateTime.Now);
                Category[] siesaCategories = await _siesaRepository.GetAllCategories();
                _ = InactiveAbsentCategories(siesaCategories); //TODO: Genera solapamiento del llamado a dbcontext si no se coloca el await 

                foreach(Category siesaCategory in siesaCategories)
                {
                    Category? localCategory = await _localRepository.GetCategoryBySiesaId(siesaCategory.SiesaId);

                    if(localCategory is not null)
                    {
                        //Recorre las categorias hijas provinientes del JSON en busca de nuevas subcategorias (líneas)
                        foreach(Category childSiesaCategory in siesaCategory.Childs)
                        {
                            Category childLocalCategory = await _localRepository.GetCategoryBySiesaId(childSiesaCategory.SiesaId);
                            if(childLocalCategory is null)
                            {
                                _logger.LogInformation("Creando linea con siesa id: {SiesaId}:{Nombre}", childSiesaCategory.SiesaId, childLocalCategory.Name);
                                childSiesaCategory.SetFather(localCategory); //Setea la categoria padre, ya que la proviniente SIESA tiene padre con Id nulo
                                await SaveCategory(childSiesaCategory);
                            }
                        }
                    }else{
                        _logger.LogInformation("Creando familia con siesa id: {SiesaId}:{Nombre}", siesaCategory.SiesaId, siesaCategory.Name);
                        await SaveCategory(localCategory);
                        foreach (Category localChildCategory in localCategory.Childs)
                        {
                            _logger.LogInformation("Creando linea con siesa id: {SiesaId}:{Nombre}", localChildCategory.SiesaId, localChildCategory.Name);
                            await SaveCategory(localChildCategory, true);
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Error renderizando categorías: {Message}, stack: {Stack}", exception.Message, exception.StackTrace);
            }finally{
                _logger.LogTrace("Finalizando renderizado de categorías, fecha: {Date}", DateTime.Now);
                _mail.sendMail(this._loadCategories, this._inactivatedCategories, this._failedLoadCategories);
            }
        }

        /// <summary>
        /// Desactiva las lineas y/o familias (categorias) que ya no esten en el JSON
        /// Proviniente de SIESA
        /// </summary>
        /// <param name="currentSiesaCategories">Lista de categorias actuales del JSON de SIESA</param>
        /// <returns>Retorna las categorias desactivadas</returns>
        public async Task InactiveAbsentCategories(Category[] currentSiesaCategories)
        {
            try
            {
                Category[] deltaCategories = await this._localRepository.GetDeltaCategories(currentSiesaCategories);
                foreach (Category deltaCategory in deltaCategories)
                {
                    deltaCategory.IsActive = false;
                    int vtexId = deltaCategory.VtexId ?? throw new InvalidOperationException("VtexId nulo");
                    bool updated = await _vtexRepository.UpdateCategoryState(vtexId, false);
                    if(updated){
                        await _localRepository.UpdateCategory(deltaCategory);
                        _inactivatedCategories.Add(deltaCategory);
                    }
                }
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Error obteniendo las categorías ausentes: {Message}, stack: {Stack}", exception.Message, exception.StackTrace);
            }
        }

        /// <summary>
        /// Guarda la categoría en VTEX y en base de datos
        /// Si no existe el padre o su Id es nulo, se crea un nuevo registro en la base de datos
        /// </summary>
        /// <param name="category">Nueva categoría a crear</param>
        /// <param name="alreadyExistsInBd">Indica si la categoría ya existe en la base de datos, en caso de que si, solo actualiza</param>
        /// <returns></returns>
        public async Task SaveCategory(Category category, bool alreadyExistsInBd = false){
            try{
                if(alreadyExistsInBd){
                    Category vtexCategory = await this._vtexRepository.SaveCategory(category);
                    category.VtexId = vtexCategory.VtexId;
                    await this._localRepository.UpdateCategory(category);
                    this._loadCategories.Add(category);
                }else{
                    category = await _localRepository.SaveCategory(category);
                    Category vtexCategory = await _vtexRepository.SaveCategory(category);
                    category.VtexId = vtexCategory.VtexId;
                    category = await _localRepository.UpdateCategory(category);
                    _loadCategories.Add(category);
                }
            }
            catch(Exception exception){
                _failedLoadCategories.Add(category);
                _logger.LogError(exception, "Error guardando la categoría con id siesa: {SiesaId}: {Message}, stack: {Stack}", category.SiesaId, exception.Message, exception.StackTrace);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    _loadCategories.Clear();
                    _inactivatedCategories.Clear();
                    _inactiveCategories.Clear();
                    _failedLoadCategories.Clear();
                    _notProccecedCategories.Clear();
                }
                // Dispose unmanaged resources here if any.
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RenderCategories()
        {
            Dispose(false);
        }
    }
}
