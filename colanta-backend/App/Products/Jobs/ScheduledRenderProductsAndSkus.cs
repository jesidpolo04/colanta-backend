using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace colanta_backend.App.Products.Jobs
{
    using Products.Domain;
    public class ScheduledRenderProductsAndSkus : IHostedService , IDisposable
    {
        private Timer _timer;
        private ProductsRepository productsLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusRepository skusLocalRepository;
        private SkusVtexRepository skusVtexRepository;
        private ProductsSiesaRepository productsSiesaRepository;
        public ScheduledRenderProductsAndSkus
            (
            ProductsRepository productsLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusRepository skusLocalRepository,
            SkusVtexRepository skusVtexRepository,
            ProductsSiesaRepository productsSiesaRepository
            )
        {
            this.productsLocalRepository = productsLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.skusVtexRepository = skusVtexRepository;
            this.productsSiesaRepository = productsSiesaRepository;
        }

        public async void Execute(object state)
        {
            RenderProductsAndSkus renderProductsAndSku = new RenderProductsAndSkus(
                this.productsLocalRepository, 
                this.productsVtexRepository, 
                this.skusLocalRepository, 
                this.skusVtexRepository, 
                this.productsSiesaRepository
                );
            await renderProductsAndSku.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
