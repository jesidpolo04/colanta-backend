namespace colanta_backend.App.Categories.Jobs
{
    using App.Categories.Domain;
    using App.Shared.Application;
    using App.Shared.Domain;

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using NCrontab;
    public class ScheduledUpCategoriesToVtex: IDisposable, IHostedService
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0 0/1 * * *";
        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private IProcess logs;

        public ScheduledUpCategoriesToVtex(
            CategoriesRepository localRepository,
            CategoriesVtexRepository vtexRepository,
            IProcess logs
)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logs = logs;
        }

        public async void Execute()
        {
            UpCategoriesToVtex upCategoriesToVtex = new UpCategoriesToVtex(this.localRepository, this.vtexRepository, this.logs);
            using (upCategoriesToVtex)
            {
                await upCategoriesToVtex.Invoke();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken);

                    this.Execute();

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

        public void Dispose()
        {
        }
    }
}
