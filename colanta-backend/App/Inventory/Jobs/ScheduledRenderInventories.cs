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
        private RenderInventories renderInventories;

        public ScheduledRenderInventories(RenderInventories renderInventories)
        {
            this.renderInventories = renderInventories;
        }

        public async void Execute(object state)
        {
            await renderInventories.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
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
