namespace colanta_backend.App.Products.Jobs
{
    using Products.Domain;
    using Shared.Domain;
    using Shared.Application;
    using System;
    using System.Threading.Tasks;
    public class UpdateProductsAndSkusStates
    {
        private ProductsRepository productsLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusRepository skusLocalRepository;
        private SkusVtexRepository skusVtexRepository;
        private CustomConsole console = new CustomConsole();
        public UpdateProductsAndSkusStates(
            ProductsRepository productsLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusRepository skusLocalRepository,
            SkusVtexRepository skusVtexRepository

            )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.skusVtexRepository = skusVtexRepository;
        }

        public async Task Invoke()
        {
            this.console.processStartsAt("Actualizando estados de productos", DateTime.Now);
            //try
            //{
                Product[] localNotNullProducts = await this.productsLocalRepository.getVtexProducts();
                foreach (Product localNotNullVtexProduct in localNotNullProducts)
                {
                    try
                    {
                        Product vtexProduct = await this.productsVtexRepository.getProductByVtexId(localNotNullVtexProduct.vtex_id.ToString());
                        if (vtexProduct.is_active != localNotNullVtexProduct.is_active)
                        {
                            localNotNullVtexProduct.is_active = vtexProduct.is_active;
                            await this.productsLocalRepository.updateProduct(localNotNullVtexProduct);
                        }
                    }
                    catch (VtexException vtexException)
                    {

                    }
                }

                Sku[] localNotNullSkus = await this.skusLocalRepository.getVtexSkus();
                foreach (Sku localNotNullVtexSku in localNotNullSkus)
                {
                    try
                    {
                        Sku vtexSku = await this.skusVtexRepository.getSkuByVtexId(localNotNullVtexSku.vtex_id.ToString());
                        if (vtexSku.is_active != localNotNullVtexSku.is_active)
                        {
                            localNotNullVtexSku.is_active = vtexSku.is_active;
                            await this.skusLocalRepository.updateSku(localNotNullVtexSku);
                        }
                    }
                    catch (VtexException vtexException)
                    {

                    }
                }
            //}
            //catch (Exception exception)
            //{

            //}
            this.console.processEndstAt("Actualizando estados de productos", DateTime.Now);
        }
    }
}
