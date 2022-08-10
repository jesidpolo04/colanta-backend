namespace colanta_backend.App.CustomerCredit.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    public class ScheduledUpdateAccountsBalance : IHostedService, IDisposable
    {
        private Timer _timer;
        private UpdateAccountsBalance updateAccountsBalance;

        public ScheduledUpdateAccountsBalance(UpdateAccountsBalance updateAccountsBalance)
        {
            this.updateAccountsBalance = updateAccountsBalance;
        }

        public void Execute(object state)
        {
            updateAccountsBalance.Invoke().Wait();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2));
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
