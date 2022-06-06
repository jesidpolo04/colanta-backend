namespace colanta_backend.App.Inventory.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using App.Inventory.Domain;
    using App.Shared.Domain;
    using App.Shared.Application;
    public class ScheduledRenderInventories : IHostedService, IDisposable
    {
        private Timer _timer;
        private InventoriesRepository localRepository;
        private InventoriesVtexRepository vtexRepository;
        private InventoriesSiesaRepository siesaRepository;
        private WarehousesRepository warehousesRepository;
        private ILogs logs;

        public ScheduledRenderInventories
            (
            InventoriesRepository localRepository,
            InventoriesVtexRepository vtexRepository,
            InventoriesSiesaRepository siesaRepository,
            WarehousesRepository warehousesRepository,
            ILogs logs
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
            this.warehousesRepository = warehousesRepository;
            this.logs = logs;
        }

        public async void Execute(object state)
        {
            RenderInventories renderWarehouses = new RenderInventories(
                this.localRepository,
                this.vtexRepository,
                this.siesaRepository,
                this.warehousesRepository,
                this.logs
                );
            await renderWarehouses.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(50), TimeSpan.FromSeconds(60));
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
