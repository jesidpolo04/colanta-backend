using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace colanta_backend.App.Brands.Jobs
{
    using Shared.Infraestructure;
    using Shared.Application;
    using App.Brands.Domain;
    using Shared.Domain;
    public class ScheduledRenderBrands : IHostedService, IDisposable
    {
        private Timer _timer;
        private BrandsRepository brandsLocalRepository { get; set; }
        private BrandsVtexRepository brandsVtexRepository { get; set; }
        private ILogs logs { get; set; }
        private IConfiguration configuration { get; set; }
        private EmailSender emailSender { get; set; }
        public ScheduledRenderBrands(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository, ILogs logs, EmailSender emailSender, IConfiguration configuration)
        {
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandsVtexRepository = brandsVtexRepository;
            this.logs = logs;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public async void Execute(object state)
        {
            RenderBrands renderBrands = new RenderBrands(this.brandsLocalRepository, this.brandsVtexRepository, this.logs, this.emailSender, this.configuration);
            renderBrands.Invoke();
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
