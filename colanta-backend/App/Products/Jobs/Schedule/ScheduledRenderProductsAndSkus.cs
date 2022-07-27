using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace colanta_backend.App.Products.Jobs
{
    public class ScheduledRenderProductsAndSkus : IHostedService , IDisposable
    {
        private Timer _timer;
        private RenderProductsAndSkus renderProductsAndSkus;
        public ScheduledRenderProductsAndSkus(RenderProductsAndSkus renderProductsAndSkus)
        {
            this.renderProductsAndSkus = renderProductsAndSkus;
        }

        public async void Execute(object state)
        {
            await this.renderProductsAndSkus.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromMinutes(6));
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
