namespace colanta_backend.App.CustomerCredit.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    public class ScheduledRenderCreditAccounts : IHostedService, IDisposable
    {
        private Timer _timer;
        private RenderCreditAccounts renderCreditAccounts;

        public ScheduledRenderCreditAccounts(RenderCreditAccounts renderCreditAccounts)
        {
            this.renderCreditAccounts = renderCreditAccounts;
        }

        public async void Execute(object state)
        {
            using (renderCreditAccounts)
            {
                await renderCreditAccounts.Invoke();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5));
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
