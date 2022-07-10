namespace colanta_backend.App.Prices.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using App.Prices.Domain;
    using App.Shared.Domain;
    using App.Shared.Application;
    public class ScheduledRenderPrices : IHostedService, IDisposable
    {
        private Timer _timer;
        private PricesRepository localRepository;
        private PricesVtexRepository vtexRepository;
        private PricesSiesaRepository siesaRepositoy;
        private EmailSender emailSender;
        private ILogs logs;

        public ScheduledRenderPrices
        (
            PricesRepository localRepository,
            PricesVtexRepository vtexRepository,
            PricesSiesaRepository siesaRepositoy,
            EmailSender emailSender,
            ILogs logs

        )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepositoy = siesaRepositoy;
            this.emailSender = emailSender;
            this.logs = logs;
        }

        public async void Execute(object state)
        {
            RenderPrices renderPrices = new RenderPrices(
                this.localRepository,
                this.vtexRepository,
                this.siesaRepositoy,
                this.emailSender,
                this.logs
                );
            await renderPrices.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(5));
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
