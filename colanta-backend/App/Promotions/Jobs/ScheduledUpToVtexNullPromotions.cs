namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Shared.Domain;
    using Shared.Application;
    public class ScheduledUpToVtexNullPromotions : IHostedService, IDisposable
    {
        private Timer _timer;
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;

        public ScheduledUpToVtexNullPromotions(
            PromotionsRepository localRepository,
            PromotionsVtexRepository vtexRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
        }

        public async void Execute(object state)
        {
            UpToVtexNullPromotions upToVtexNullPromotions = new UpToVtexNullPromotions(
                this.localRepository,
                this.vtexRepository
                );
            await upToVtexNullPromotions.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30));
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
