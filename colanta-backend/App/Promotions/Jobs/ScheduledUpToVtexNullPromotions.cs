namespace colanta_backend.App.Promotions.Jobs
{
    using Promotions.Domain;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Shared.Domain;
    using Shared.Application;
    using NCrontab;
    public class ScheduledUpToVtexNullPromotions : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 30 0/1 * * *";
        private PromotionsRepository localRepository;
        private PromotionsVtexRepository vtexRepository;
        private ILogger logger;

        public ScheduledUpToVtexNullPromotions(
            PromotionsRepository localRepository,
            PromotionsVtexRepository vtexRepository,
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
            try
            {
                UpToVtexNullPromotions upToVtexNullPromotions = new UpToVtexNullPromotions(
                                this.localRepository,
                                this.vtexRepository
                                );
                upToVtexNullPromotions.Invoke().Wait();
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
