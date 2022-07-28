namespace colanta_backend.App.Categories.Jobs
{
    using App.Categories.Domain;
    using App.Shared.Application;
    using App.Shared.Domain;

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    public class ScheduledUpCategoriesToVtex: IDisposable, IHostedService
    {
        private Timer _timer;
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private IProcess logs;

        public ScheduledUpCategoriesToVtex(
            CategoriesRepository localRepository,
            CategoriesVtexRepository vtexRepository,
            IProcess logs
)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logs = logs;
        }

        public async void Execute(object state)
        {
            UpCategoriesToVtex upCategoriesToVtex = new UpCategoriesToVtex(this.localRepository, this.vtexRepository, this.logs);
            await upCategoriesToVtex.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1));
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
