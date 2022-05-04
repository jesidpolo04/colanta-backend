namespace colanta_backend.App.Prices.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using App.Prices.Domain;
    public class ScheduledRenderPrices : IHostedService, IDisposable
    {
        private Timer _timer;
        private PricesRepository localRepository;
        private PricesVtexRepository vtexRepository;
        private PricesSiesaRepository siesaRepositoy;

        public ScheduledRenderPrices
        (
            PricesRepository localRepository,
            PricesVtexRepository vtexRepository,
            PricesSiesaRepository siesaRepositoy
        )
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepositoy = siesaRepositoy;
        }

        public async void Execute(object state)
        {
            RenderPrices renderPrices = new RenderPrices(
                this.localRepository,
                this.vtexRepository,
                this.siesaRepositoy
                );
            await renderPrices.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60));
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
