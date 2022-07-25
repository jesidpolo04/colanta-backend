namespace colanta_backend.App.Categories.Jobs
{
    using App.Categories.Domain;
    using App.Shared.Application;
    using App.Shared.Domain;
    
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
            this.renderCategories.Invoke();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Execute, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
