namespace colanta_backend.App.Brands.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Brands.Domain;
    using NCrontab;
    using Shared.Domain;
    public class ScheduledUpBrandsToVtex : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0 0/1 * * *";
        private BrandsRepository brandsLocalRepository { get; set; }
        private BrandsVtexRepository brandVtexRepository { get; set; }
        private ILogger logger;
        public ScheduledUpBrandsToVtex(BrandsRepository brandsLocalRepository, BrandsVtexRepository brandsVtexRepository, ILogger logger)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.brandsLocalRepository = brandsLocalRepository;
            this.brandVtexRepository = brandsVtexRepository;
            this.logger = logger;
        }
        public void Execute()
        {
            try
            {
                UpBrandsToVtex upBrandsToVtex = new UpBrandsToVtex(this.brandsLocalRepository, this.brandVtexRepository);
                upBrandsToVtex.Invoke().Wait();
            }
            catch(Exception exception)
            {
                this.logger.writelog(exception);
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
