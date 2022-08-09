namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;
    public class ScheduledMapFamiliesToStore : IHostedService, IDisposable
    {
        private Timer _timer;
        private MapFamiliesToStore task;
        public ScheduledMapFamiliesToStore(MapFamiliesToStore task)
        {
            this.task = task;
        }

        public async void Execute(object state)
        {
            await this.task.Invoke();
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
