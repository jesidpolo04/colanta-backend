namespace colanta_backend.App.Promotions.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Shared.Domain;
    using NCrontab;
    public class ScheduledRenderPromotions : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 30 0 * * *";
        private RenderPromotions renderPromotions;
        private ILogger logger;
        public ScheduledRenderPromotions(RenderPromotions renderPromotions, ILogger logger)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.renderPromotions = renderPromotions;
            this.logger = logger;
        }

        public void Execute()
        {
            using (renderPromotions)
            {
                try
                {
                    this.renderPromotions.Invoke().Wait();
                }
                catch (Exception exception)
                {
                    this.logger.writelog(exception);
                }
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
