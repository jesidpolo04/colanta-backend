using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace colanta_backend.App.Brands.Jobs
{
    public class ScheduledRenderBrands : IHostedService, IDisposable
    {
        private Timer _timer;
        private RenderBrands renderBrands;
        public ScheduledRenderBrands(RenderBrands renderBrands)
        {
            this.renderBrands = renderBrands;
        }

        public async void Execute(object state)
        {
            await this.renderBrands.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));
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
