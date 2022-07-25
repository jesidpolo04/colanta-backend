using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace colanta_backend.App.Products.Jobs
{
    using Products.Domain;
    using Shared.Application;
    using Shared.Domain;
    public class ScheduledUpToVtexNullProductsAndSkus: IHostedService , IDisposable
    {
        private Timer _timer;
        private ProductsRepository productsLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusRepository skusLocalRepository;
        private SkusVtexRepository skusVtexRepository;
        public ScheduledUpToVtexNullProductsAndSkus(
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

        public async void Execute(object state)
        {
            UpToVtexNullProductsAndSkus upToVtexNullProductsAndSkus = new UpToVtexNullProductsAndSkus(
                this.productsLocalRepository,
                this.skusLocalRepository,
                this.productsVtexRepository,
                this.skusVtexRepository
                );
            await upToVtexNullProductsAndSkus.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(30));
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
