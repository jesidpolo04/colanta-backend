namespace colanta_backend.App.CustomerCredit.Jobs
{
    using Microsoft.Extensions.Hosting;
    using NCrontab;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    public class ScheduledReduceVtexCreditLimit : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0/5 * * * *"; // run each 5 min
        private readonly ReduceVtexCreditLimit _task;

        public ScheduledReduceVtexCreditLimit(ReduceVtexCreditLimit task)
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            _task = task;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken); // wait until next time

                    await _task.Invoke(); //execute some task

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
