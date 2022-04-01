using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace colanta_backend.App.Brands.Jobs
{
    using Shared.Infraestructure;
    using Shared.Application;
    using App.Brands.Domain;
    public class ScheduledRenderBrands : IHostedService, IDisposable
    {
        private Timer _timer;
        private BrandsRepository brandsLocalRepository { get; set; }
        private BrandsVtexRepository brandsVtexRepository { get; set; }
        private ILogs logs { get; set; }
        public ScheduledRenderBrands(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository, ILogs logs)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
            this.logs = logs;
        }

        public async void Execute(object state)
        {
            RenderBrands renderBrands = new RenderBrands(this.brandsLocalRepository, this.brandsVtexRepository, this.logs);
            renderBrands.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
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
