namespace colanta_backend.App.Inventory.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using App.Inventory.Domain;
    public class ScheduledRenderWarehouses : IHostedService, IDisposable
    {
        private Timer _timer;
        private WarehousesRepository localRepository;
        private WarehousesSiesaVtexRepository siesaVtexRepository;

        public ScheduledRenderWarehouses
        (
            WarehousesRepository localRepository,
            WarehousesSiesaVtexRepository siesaVtexRepository
        )
        {
            this.localRepository = localRepository;
            this.siesaVtexRepository = siesaVtexRepository;
        }

        public async void Execute(object state)
        {
            RenderWarehouses renderWarehouses = new RenderWarehouses(
                this.siesaVtexRepository,
                this.localRepository
                );
            await renderWarehouses.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(40), TimeSpan.FromSeconds(60));
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
