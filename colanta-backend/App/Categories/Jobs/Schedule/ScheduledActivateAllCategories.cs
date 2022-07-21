namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;
    public class ScheduledActivateAllCategories : IHostedService, IDisposable
    {
        private Timer _timer;
        private ActivateAllCategories activateAllCategories;
        public ScheduledActivateAllCategories(ActivateAllCategories activateAllCategories)
        {
            this.activateAllCategories = activateAllCategories;
        }

        public async void Execute(object state)
        {
            await this.activateAllCategories.Invoke();
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
