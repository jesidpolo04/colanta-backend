namespace colanta_backend.App.Prices.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    public class ScheduledRenderPrices : IHostedService, IDisposable
    {
        private Timer _timer;
        private RenderPrices renderPrices;

        public ScheduledRenderPrices(RenderPrices renderPrices)
        {
            this.renderPrices = renderPrices;
        }

        public async void Execute(object state)
        {
            await this.renderPrices.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(4), TimeSpan.FromMinutes(5));
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
