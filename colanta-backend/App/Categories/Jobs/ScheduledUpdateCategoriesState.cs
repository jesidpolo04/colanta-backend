namespace colanta_backend.App.Categories.Jobs
{
    using App.Categories.Domain;
    using App.Shared.Application;
    using App.Shared.Domain;

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    public class ScheduledUpdateCategoriesState: IDisposable, IHostedService
    {
        private Timer _timer;
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;

        public ScheduledUpdateCategoriesState(
            CategoriesRepository localRepository,
            CategoriesVtexRepository vtexRepository)
        {
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
        }

        public async void Execute(object state)
        {
            UpdateCategoriesState updateCategoriesState = new UpdateCategoriesState(this.localRepository, this.vtexRepository);
            await updateCategoriesState.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(60));
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
