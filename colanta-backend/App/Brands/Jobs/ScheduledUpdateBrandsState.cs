namespace colanta_backend.App.Brands.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Brands.Domain;
    public class ScheduledUpdateBrandsState : IHostedService, IDisposable
    {
        private Timer _timer;
        private BrandsRepository brandsLocalRepository { get; set; }
        private BrandsVtexRepository brandVtexRepository { get; set; }
        public ScheduledUpdateBrandsState(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandVtexRepository = brandsVtexRepository;
        }
        public async void Execute(object state)
        {
            UpdateBrandsState updateBrandsState = new UpdateBrandsState(this.brandsLocalRepository, this.brandVtexRepository);
            updateBrandsState.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30));
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
