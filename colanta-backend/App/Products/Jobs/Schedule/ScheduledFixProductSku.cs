namespace colanta_backend.App.Products.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ScheduledFixProductSku : IHostedService, IDisposable
    {
        private Timer _timer;
        private FixProductSkus _task;

        public ScheduledFixProductSku(FixProductSkus task)
        {
            _task = task;
        }

        public async void Execute(object state)
        {
            await this._task.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
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
