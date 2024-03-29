﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace colanta_backend.App.Products.Jobs
{
    using Products.Domain;
    using Shared.Application;
    using Shared.Domain;
    using NCrontab;
    using Shared.Domain;
    public class ScheduledUpdateProductsAndSkusStates : IHostedService , IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private DateTime _nextRun;
        private const string Schedule = "0 0/60 * * * *";
        private ProductsRepository productsLocalRepository;
        private ProductsVtexRepository productsVtexRepository;
        private SkusRepository skusLocalRepository;
        private SkusVtexRepository skusVtexRepository;
        private ILogger logger;
        
        public ScheduledUpdateProductsAndSkusStates(
            ProductsRepository productsLocalRepository,
            ProductsVtexRepository productsVtexRepository,
            SkusRepository skusLocalRepository,
            SkusVtexRepository skusVtexRepository,
            ILogger logger
            )
        {
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            this.productsLocalRepository = productsLocalRepository;
            this.productsVtexRepository = productsVtexRepository;
            this.skusLocalRepository = skusLocalRepository;
            this.skusVtexRepository = skusVtexRepository;
            this.logger = logger;
        }

        public void Execute()
        {
            try
            {
                UpdateProductsAndSkusStates updateProductsAndSkusStates = new UpdateProductsAndSkusStates(
                this.productsLocalRepository,
                this.productsVtexRepository,
                this.skusLocalRepository,
                this.skusVtexRepository
                );
                updateProductsAndSkusStates.Invoke().Wait();
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
