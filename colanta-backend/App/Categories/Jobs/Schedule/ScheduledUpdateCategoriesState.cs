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
    public class ScheduledUpdateCategoriesState: IDisposable, IHostedService
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0/30 * * * *";

        private CategoriesRepository localRepository;
        private CategoriesVtexRepository vtexRepository;
        private ILogger logger;

        public ScheduledUpdateCategoriesState(
            CategoriesRepository localRepository,
            CategoriesVtexRepository vtexRepository,
            ILogger logger)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.localRepository = localRepository;
            this.vtexRepository = vtexRepository;
            this.logger = logger;
        }

        public void Execute()
        {
            UpdateCategoriesState updateCategoriesState = new UpdateCategoriesState(this.localRepository, this.vtexRepository, this.logger);
            updateCategoriesState.Invoke().Wait();
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
