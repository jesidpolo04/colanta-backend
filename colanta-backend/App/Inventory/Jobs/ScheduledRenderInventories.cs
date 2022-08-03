namespace colanta_backend.App.Inventory.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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
            using (renderInventories)
            {
                await renderInventories.Invoke();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Start Async ... :D");
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
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
