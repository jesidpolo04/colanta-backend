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
    public class ScheduledRenderPromotions : IHostedService, IDisposable
    {
        private Timer _timer;
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;
        private PromotionsSiesaRepository siesaRepositoy;
        private ILogs logs;
        public ScheduledRenderPromotions
        (
            PromotionsRepository localRepository,
            PromotionsVtexRepository vtexRepository,
            PromotionsSiesaRepository siesaRepositoy,
            ILogs logs
        )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepositoy = siesaRepositoy;
            this.logs = logs;
        }

        public async void Execute(object state)
        {
            RenderPromotions renderPromotions = new RenderPromotions(
                this.localRepository,
                this.vtexRepository,
                this.siesaRepositoy,
                this.logs
                );
            await renderPromotions.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
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
