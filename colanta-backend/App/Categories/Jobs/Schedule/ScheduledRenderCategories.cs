namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using NCrontab;
    public class ScheduledRenderCategories : IHostedService, IDisposable
    {

        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0 0/2 * * *";
        private RenderCategories renderCategories;
        public ScheduledRenderCategories(RenderCategories renderCategories)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.renderCategories = renderCategories;
        }

        public async void Execute()
        {
            using (renderCategories)
            {
                await this.renderCategories.Invoke();
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
            this.renderCategories.Dispose();
        }
    }
}
