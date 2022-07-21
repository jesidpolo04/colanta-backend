namespace colanta_backend.App.Products.Jobs
{
    using Products.Domain;
    using System.Threading.Tasks;
    using System;
    public class FixProductSkus
    {
        private SkusRepository _skuRepository;
        private SkusVtexRepository _skuVtexRepository;

        public FixProductSkus(SkusRepository skuRepository, SkusVtexRepository skuVtexRepository)
        {
            _skuRepository = skuRepository;
            _skuVtexRepository = skuVtexRepository;
        }

        public async Task Invoke()
        {
            Sku[] skus = await _skuRepository.getVtexSkus();

            foreach(Sku sku in skus)
            {
                Console.WriteLine($"Cambiando el producto vtex id del sku {sku.concat_siesa_id} de {sku.product_id} a {sku.product.vtex_id}");
                await _skuVtexRepository.updateSku(sku);
            }
        }
    }
}
