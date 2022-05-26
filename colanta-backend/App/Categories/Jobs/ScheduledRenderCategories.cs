namespace colanta_backend.App.Categories.Jobs
{
    using App.Categories.Domain;
    using App.Shared.Application;
    using App.Shared.Domain;
    
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    public class ScheduledRenderCategories : IHostedService, IDisposable
    {
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private CategoriesSiesaRepository siesaRepository;
        private ILogs logs;
        private EmailSender emailSender;

        private Timer _timer;
        public ScheduledRenderCategories(
            CategoriesRepository localRepository, 
            CategoriesVtexRepository vtexRepository, 
            CategoriesSiesaRepository siesaReposiory,
            ILogs logs,
            EmailSender emailSender)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.siesaRepository = siesaReposiory;
            this.logs = logs;
            this.emailSender = emailSender;
        }

        public async void Execute(object state)
        {
            RenderCategories renderCategories = new RenderCategories(this.localRepository, this.vtexRepository, this.siesaRepository, this.logs, this.emailSender);
            await renderCategories.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(60));
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
