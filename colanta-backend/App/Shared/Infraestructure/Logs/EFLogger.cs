namespace colanta_backend.App.Shared.Infraestructure
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    public class EFLogger : Domain.ILogger
    {
        private ColantaContext dbContext;
        public EFLogger(IConfiguration configuration)
        {
            this.dbContext = new ColantaContext(configuration);
        }
        public Task writelog(Exception exception)
        {
            EFLog log = new EFLog();
            log.message = exception.Message;
            log.stack = exception.StackTrace;
            log.exception = exception.ToString();

            this.dbContext.Logs.Add(log);
            this.dbContext.SaveChanges();
            return Task.CompletedTask;
        }
        
        
    }
}
