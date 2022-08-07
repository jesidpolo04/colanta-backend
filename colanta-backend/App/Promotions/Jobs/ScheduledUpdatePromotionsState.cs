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
    public class ScheduledUpdatePromotionsState : IHostedService, IDisposable
    {
        private Timer _timer;
        private UpdatePromotionsState updatePromotionsState;
        private TimeSpan timeout = TimeSpan.FromSeconds(5);
        private TimeSpan interval = TimeSpan.FromMinutes(15);
        public ScheduledUpdatePromotionsState(UpdatePromotionsState updatePromotionsState)
        {
            this.updatePromotionsState = updatePromotionsState;
        }

        public async void Execute(object state)
        {
            await this.updatePromotionsState.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, timeout, interval);
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
