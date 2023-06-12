namespace colanta_backend.App.Products.Jobs
{
    using System;
    using System.Threading.Tasks;
    using Products.Domain;
    using Shared.Domain;
    using Shared.Application;
    public class UpToVtexNullProductsAndSkus
    {
        private ProductsRepository productsLocalRepository;
        private SkusRepository skusLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusVtexRepository skusVtexRepository;
        private ILogger logger;
        public UpToVtexNullProductsAndSkus(
            ProductsRepository productsLocalRepository,
            SkusRepository skusLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusVtexRepository skusVtexRepository,
            ILogger logger
            )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusVtexRepository = skusVtexRepository;
            this.logger = logger;
        }

        public async Task Invoke()
        {
            try
            {
                Product[] nullProducts = await this.productsLocalRepository.getVtexNullProducts();
                foreach(Product nullLocalproduct in nullProducts)
                {
                    Product vtexProduct = await this.productsVtexRepository.saveProduct(nullLocalproduct);
                    nullLocalproduct.vtex_id = vtexProduct.vtex_id;
                    await this.productsLocalRepository.updateProduct(nullLocalproduct);
                }

                Sku[] nullSkus = await this.skusLocalRepository.getVtexNullSkus();
                foreach (Sku nullLocalSku in nullSkus)
                {
                    Sku vtexSku = await this.skusVtexRepository.saveSku(nullLocalSku);
                    nullLocalSku.vtex_id = vtexSku.vtex_id;
                    await this.skusLocalRepository.updateSku(nullLocalSku);
                }
            }
            catch(Exception exception)
            {
                await this.logger.writelog(exception);
            }
        }
    }
}
