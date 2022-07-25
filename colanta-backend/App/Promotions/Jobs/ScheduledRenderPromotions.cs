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
        private RenderPromotions renderPromotions;
        public ScheduledRenderPromotions(RenderPromotions renderPromotions)
        {
            this.renderPromotions = renderPromotions;
        }

        public async void Execute(object state)
        {
            await this.renderPromotions.Invoke();
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
