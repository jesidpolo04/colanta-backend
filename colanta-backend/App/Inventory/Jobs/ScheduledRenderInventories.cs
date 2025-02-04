namespace colanta_backend.App.Inventory.Jobs
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NCrontab;
    using Shared.Domain;

    public class ScheduledRenderInventories : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 */5 * * * *";
        private RenderInventories renderInventories;
        private ILogger logger;

        public ScheduledRenderInventories(RenderInventories renderInventories, ILogger logger)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.renderInventories = renderInventories;
            this.logger = logger;
        }

        public void Execute()
        {
            using (renderInventories)
            {
                try
                {
                    renderInventories.Invoke().Wait();
                }
                catch(Exception exception)
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
