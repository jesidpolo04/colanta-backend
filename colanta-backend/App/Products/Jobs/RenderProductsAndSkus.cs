namespace colanta_backend.App.Products.Jobs
{
    using System.Threading.Tasks;
    using Products.Application;
    using Products.Domain;
    using System;
    public class RenderProductsAndSkus
    {
        private ProductsRepository productsLocalRepository;
        private SkusRepository skusLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusVtexRepository skusVtexRepository;
        private ProductsSiesaRepository siesaRepository;
        public RenderProductsAndSkus
        (
            ProductsRepository productsLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusRepository skusLocalRepository,
            SkusVtexRepository skusVtexRepository,
            ProductsSiesaRepository siesaRepository
        )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusVtexRepository = skusVtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async Task<bool> Invoke()
        {
            GetAllProductsFromSiesa getAllProductsFromSiesa = new GetAllProductsFromSiesa(this.siesaRepository);
            GetAllSkusFromSiesa getAllSkusFromSiesa = new GetAllSkusFromSiesa(this.siesaRepository);
            GetDeltaProducts getDeltaProducts = new GetDeltaProducts(this.productsLocalRepository);
            GetDeltaSkus getDeltaSkus = new GetDeltaSkus(this.skusLocalRepository);
            GetSkuBySiesaId getSkuBySiesaId = new GetSkuBySiesaId(this.skusLocalRepository);
            GetVtexSkuBySiesaId getVtexSkuBySiesaId = new GetVtexSkuBySiesaId(this.skusVtexRepository);
            GetProductBySiesaId getProductBySiesaId = new GetProductBySiesaId(this.productsLocalRepository);
            GetVtexProductBySiesaId getVtexProductBySiesaId = new GetVtexProductBySiesaId(this.productsVtexRepository);
            UpdateProduct updateProduct = new UpdateProduct(this.productsLocalRepository);
            UpdateProducts updateProducts = new UpdateProducts(this.productsLocalRepository);
            UpdateSku updateSku = new UpdateSku(this.skusLocalRepository);
            UpdateSkus updateSkus = new UpdateSkus(this.skusLocalRepository);
            UpdateVtexProduct updateVtexProduct = new UpdateVtexProduct(this.productsVtexRepository);
            UpdateVtexSku updateVtexSku = new UpdateVtexSku(this.skusVtexRepository);
            SaveProduct createProduct = new SaveProduct(this.productsLocalRepository);
            SaveSku createSku = new SaveSku(this.skusLocalRepository);
            SaveVtexProduct createVtexProduct = new SaveVtexProduct(this.productsVtexRepository);
            SaveVtexSku createVtexSku = new SaveVtexSku(this.skusVtexRepository);

            //1.obtener todos los productos y skus de siesa
            Product[] allSiesaProducts = await getAllProductsFromSiesa.Invoke();
            Sku[] allSiesaSkus = await getAllSkusFromSiesa.Invoke();

            //2.desactivar los productos y skus que ya no vinieron
            Product[] deltaProducts = await getDeltaProducts.Invoke(allSiesaProducts);
            Sku[] deltaSkus = await getDeltaSkus.Invoke(allSiesaSkus);

            foreach(Product deltaProduct in deltaProducts)
            { 
                deltaProduct.is_active = false;
                //await updateVtexProduct.Invoke(deltaProduct);
            }
            await updateProducts.Invoke(deltaProducts);

            foreach(Sku deltaSku in deltaSkus)
            {
                deltaSku.is_active = false;
                //await updateVtexSku.Invoke(deltaSku);
            }
            await updateSkus.Invoke(deltaSkus);

            //3.iterar sobre cada producto y sus skus
            //4.si el producto ya existe localmente y en vtex y ademas está inactivo hay que activar
            //5.si el producto ya existe localmente y en vtex y ademas está activo todo ok
            //6.si el producto no existe en el local crearlo y crearlo en vtex si no existe

            foreach (Product siesaProduct in allSiesaProducts)
            {
                Product? localProduct = await getProductBySiesaId.Invoke(siesaProduct.siesa_id);

                if (localProduct != null && localProduct.is_active == false)
                {
                    //hay que activar
                }

                if(localProduct != null && localProduct.is_active == true)
                {
                    //todo ok
                }

                if(localProduct == null)
                {
                    localProduct = await createProduct.Invoke(siesaProduct);
                    Product vtexProduct = await createVtexProduct.Invoke(localProduct);
                    localProduct.vtex_id = vtexProduct.vtex_id;
                    await updateProduct.Invoke(localProduct);
                }
            }

            foreach (Sku siesaSku in allSiesaSkus)
            {
                Sku? localSku = await getSkuBySiesaId.Invoke(siesaSku.siesa_id);

                if (localSku != null && localSku.is_active == false)
                {
                    //hay que activar
                }

                if (localSku != null && localSku.is_active == true)
                {
                    //todo ok
                }

                if (localSku == null)
                {
                    localSku = await createSku.Invoke(siesaSku);
                    Sku vtexSku = await createVtexSku.Invoke(localSku);
                    localSku.vtex_id = vtexSku.vtex_id;
                    await updateSku.Invoke(localSku);
                }
            }

            return true;
        }
    }
}
