namespace colanta_backend.App.Promotions.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shared.Domain;
    using NCrontab;
    public class ScheduledCleanFixedPrices : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0 0/6 * * *";
        private CleanFixedPricesFromExpiredPromotion task;
        private ILogger logger;
        public ScheduledCleanFixedPrices(CleanFixedPricesFromExpiredPromotion task, ILogger logger)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.task = task;
            this.logger = logger;
        }

        public async Task Execute()
        {
            await task.Execute();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken);

                    await Execute();

                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
