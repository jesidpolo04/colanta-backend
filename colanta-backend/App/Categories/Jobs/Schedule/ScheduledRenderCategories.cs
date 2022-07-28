namespace colanta_backend.App.Categories.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    public class ScheduledRenderCategories : IHostedService, IDisposable
    {

        private Timer _timer;
        private RenderCategories renderCategories;
        public ScheduledRenderCategories(RenderCategories renderCategories)
        {
            this.renderCategories = renderCategories;
        }

        public async void Execute(object state)
        {
            using (renderCategories)
            {
                await this.renderCategories.Invoke();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.renderCategories.Dispose();
            _timer?.Dispose();
        }
    }
}
