namespace colanta_backend.App.CustomerCredit.Jobs
{
    using CustomerCredit.Domain;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Shared.Domain;
    using Shared.Application;
    public class ScheduledRenderCreditAccounts : IHostedService, IDisposable
    {
        private Timer _timer;
        private CreditAccountsRepository localRepository;
        private CreditAccountsVtexRepository vtexRepository;
        private CreditAccountsSiesaRepository siesaRepository;

        public ScheduledRenderCreditAccounts(
                CreditAccountsRepository localRepository,
                CreditAccountsVtexRepository vtexRepository,
                CreditAccountsSiesaRepository siesaRepository
            )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaRepository;
        }

        public async void Execute(object state)
        {
            RenderCreditAccounts renderAccounts = new RenderCreditAccounts(
                this.localRepository,
                this.vtexRepository,
                this.siesaRepository
                );
            await renderAccounts.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(25), TimeSpan.FromMinutes(30));
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
