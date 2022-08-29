using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using NCrontab;

namespace colanta_backend.App.Products.Jobs
{
    public class ScheduledRenderProductsAndSkus : IHostedService , IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 10 0/2 * * *";
        private RenderProductsAndSkus renderProductsAndSkus;
        public ScheduledRenderProductsAndSkus(RenderProductsAndSkus renderProductsAndSkus)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.renderProductsAndSkus = renderProductsAndSkus;
        }

        public async void Execute()
        {
            using (renderProductsAndSkus)
            {
                await this.renderProductsAndSkus.Invoke();
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
